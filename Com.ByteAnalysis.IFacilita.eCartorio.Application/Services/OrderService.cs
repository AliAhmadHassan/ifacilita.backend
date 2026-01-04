using AutoMapper;
using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class OrderService : ServiceBase<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IeCartorioClient _ieCartorioClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IKitService _kitService;
        private readonly ICategoryService _categoryService;

        IS3 s3;

        public OrderService(IKitService kitService, ICategoryService categoryService, IRepositoryBase<Order> repository, IOrderRepository orderRepository, IeCartorioClient ieCartorioClient, IMapper mapper, IConfiguration configuration) : base(repository)
        {
            _repository.SetNameCollection("order");
            _orderRepository = orderRepository;
            _ieCartorioClient = ieCartorioClient;
            _configuration = configuration;
            _categoryService = categoryService;
            _kitService = kitService;
            _mapper = mapper;
            this.s3 = new Common.Impl.S3();
        }

        public async Task<OrderInput> CreateByApplicantAsync(RequerenteInput requerenteInput)
        {
            //Obter Kit
            var kitName = _configuration["eCartorio:KitNameDefault"];
            var kitCode = Convert.ToInt32(_configuration["eCartorio:KitIdDefault"]);
            var city = _configuration["eCartorio:CityDefault"];
            var categoryCode = Convert.ToInt32(_configuration["eCartorio:CategoryCodeDefault"]);

            var kit = await _kitService.GetCertificatesByKitAsync(kitCode, requerenteInput.PropertyDetails.Municipio);
            var certificates = await _categoryService.CertificatesByCategory(categoryCode, city);

            //Preencher Atos
            List<Models.ExternalServices.Ato> atos = new List<Models.ExternalServices.Ato>();

            foreach (var ato in kit)
            {
                if (ato.Cartorios == null || ato.Cartorios.Count() == 0 || ato.Cartorios.FirstOrDefault() == null)
                {
                    var actRegistry = requerenteInput.ActRegistry.FirstOrDefault(x => x.ActId == ato.AtoId);

                    if (actRegistry == null)
                        throw new BadRequestException("O kit selecionado possui atos que não foram informado cartório(s)");

                    if (actRegistry.Registry == null || actRegistry.Registry.Length == 0)
                        throw new BadRequestException("O kit selecionado possui atos que não foram informado cartório(s)");

                    List<int?> cats = new List<int?>();
                    cats.AddRange(actRegistry.Registry);
                    ato.Cartorios = cats;
                }

                List<string> cartorios = new List<string>();
                if (ato.Cartorios != null)
                    foreach (var cart in ato.Cartorios)
                        cartorios.Add(cart?.ToString());

                object dadosAto = new object();
                if (ato.TipoAto == "Imovel")
                    dadosAto = _mapper.Map<DadosAtoImovel>(requerenteInput.PropertyDetails);
                else
                    dadosAto = _mapper.Map<IEnumerable<DadosAtoBuscar>>(requerenteInput.DataActSearch);

                if (cartorios.Count > 0)
                    atos.Add(new Models.ExternalServices.Ato()
                    {
                        AtoDescription = ato.AtoDescricao,
                        Cartorios = cartorios,
                        Municipio = city,
                        DadosAto = dadosAto
                    });
            }

            //Gerar Pedido
            PedidoSolicitarRequest order = new PedidoSolicitarRequest()
            {
                Atos = atos,
                Requerente = new Models.ExternalServices.Requerente()
                {
                    Cnpj = requerenteInput.Cnpj,
                    Cpf = requerenteInput.Cpf,
                    Email = requerenteInput.Email,
                    Nome = requerenteInput.Nome
                }
            };

            var responseCreateCartorio = await _ieCartorioClient.PedidoSolicitarAsync(order);
            var responseGetCartorio = await _ieCartorioClient.PedidoConsultarAsync(responseCreateCartorio.NumeroPedido.ToString());

            var orderForCreated = _mapper.Map<Order>(responseGetCartorio);
            orderForCreated.UrlCallback = requerenteInput.UrlCallback;
            orderForCreated.OrderCompleted = false;

            orderForCreated.Requerente = new Domain.Entities.Requerente() {
                Cnpj = requerenteInput.Cnpj,
                Cpf = requerenteInput.Cpf,
                Email = requerenteInput.Email,
                Nome = requerenteInput.Nome
            };

            var orderCreated = await _orderRepository.CreateAsync(orderForCreated);

            try
            {
                var responseBase64 = await _ieCartorioClient.PedidoBaixarBoletoAsync(responseCreateCartorio.NumeroPedido.ToString());
                if (!string.IsNullOrEmpty(responseBase64))
                {
                    orderCreated.Billet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(responseBase64, ".pdf");
                    await _orderRepository.UpdateAsync(orderCreated.Id, orderCreated);
                }
            }
            catch (eCartorioException eCatEx)
            {
                orderCreated.Billet = $"Houve uma falha ao tentar recuperar o boleto. Data/Hora: {DateTime.Now}. eCartorioException: " + eCatEx.Message;
            }
            catch (Exception ex)
            {
                orderCreated.Billet = $"Houve uma falha ao tentar recuperar o boleto. Data/Hora: {DateTime.Now}. " + ex.Message;
            }
            finally
            {
                await _orderRepository.UpdateAsync(orderCreated.Id, orderCreated);
            }

            return _mapper.Map<OrderInput>(orderCreated);
        }

        public async Task<OrderInput> CreateOrderAsync(OrderInput orderInput)
        {
            var orderMapped = _mapper.Map<PedidoSolicitarRequest>(orderInput);
            var responseCreateCartorio = await _ieCartorioClient.PedidoSolicitarAsync(orderMapped);
            var responseGetCartorio = await _ieCartorioClient.PedidoConsultarAsync(responseCreateCartorio.NumeroPedido.ToString());
            var orderInputResponse = _mapper.Map<OrderInput>(responseGetCartorio);

            var order = _mapper.Map<Order>(responseGetCartorio);
            await _orderRepository.CreateAsync(order);

            return orderInputResponse;
        }

        public async Task<OrderInput> GetOrderAsync(string id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
                throw new NotFoundException($"Order {id} not found");

            var ordereCartorio = await _ieCartorioClient.PedidoConsultarAsync(order.NumeroPedido.ToString());

            if (order == null)
                throw new NotFoundException($"Order {id} not found");

            if (string.IsNullOrEmpty(order.Billet))
            {
                try
                {
                    var responseBase64 = await _ieCartorioClient.PedidoBaixarBoletoAsync(order.NumeroPedido.ToString());
                    if (!string.IsNullOrEmpty(responseBase64))
                    {
                        order.Billet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(responseBase64, ".pdf");
                        await _orderRepository.UpdateAsync(order.Id, order);
                    }
                }
                catch (Exception ex)
                {
                    order.Billet = $"Falha ao tentar recuperar dados do boleto do pedido. Data/Hora: {DateTime.Now}" + ex.Message;
                }
            }

            foreach (var act in order.Atos)
            {
                try
                {
                    if (string.IsNullOrEmpty(act.UrlAct))
                    {
                        var actCerp = await _ieCartorioClient.AtoBaixarAtoBase64Async(act.Cerp);
                        if (!string.IsNullOrEmpty(actCerp))
                            act.UrlAct = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(actCerp, ".pdf");
                    }
                }
                catch { }

                var acteCartorio = ordereCartorio.Atos.FirstOrDefault(x => x.NumeroAto == act.NumeroAto);
                if (acteCartorio != null)
                {
                    act.Status = acteCartorio.Status;
                    act.DataPagamento = acteCartorio.DataPagamento;
                }
            }

            order.OrderCompleted = !order.Atos.Any(x => string.IsNullOrEmpty(x.UrlAct));
            if (order.OrderCompleted)
            {
                if (string.IsNullOrEmpty(order.AtosCompactados))
                {
                    try
                    {
                        var file = await _ieCartorioClient.PedidoBaixarAtosAsync(order.NumeroPedido.ToString());
                        if (!string.IsNullOrEmpty(file))
                            order.AtosCompactados = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(file, ".zip");
                    }
                    catch
                    {
                        order.AtosCompactados = "not exists";
                    }
                }
            }

            await _orderRepository.UpdateAsync(order.Id, order);
            return _mapper.Map<OrderInput>(order);
        }

        public async Task<OrderInput> GetOrderByCerpAsync(string cerp) => _mapper.Map<OrderInput>(await _orderRepository.GetOrderByCerpAsync(cerp));
    }
}
