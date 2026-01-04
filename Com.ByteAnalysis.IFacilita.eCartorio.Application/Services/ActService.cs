using AutoMapper;
using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class ActService : IActService
    {
        private readonly IMapper _mapper;
        private readonly IeCartorioClient _ieCartorioClient;
        private readonly IOrderService _orderService;

        IS3 s3;

        public ActService(IMapper mapper, IeCartorioClient ieCartorioClient, IOrderService orderService)
        {
            _mapper = mapper;
            _ieCartorioClient = ieCartorioClient;
            _orderService = orderService;

            this.s3 = new Common.Impl.S3();
        }

        public async Task<string> DownloadAsync(string cerp)
        {
            if (string.IsNullOrEmpty(cerp))
                throw new BadRequestException("O cerp é obrigatório.");

            var result = string.Empty;
            var response = await _ieCartorioClient.AtoBaixarAtoBase64Async(cerp);

            if (response == null)
                throw new NotFoundException("Ato não encontrado");

            var order = await _orderService.GetOrderByCerpAsync(cerp);
            result = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(response, ".pdf");

            if (order != null)
            {
                var act = order.Atos.FirstOrDefault(x => x.Cerp == cerp);
                if (act != null)
                {
                    act.UrlAct = result;
                    await _orderService.UpdateAsync(order.Id, _mapper.Map<Order>(order));
                }
            }

            return result;
        }

        public async Task<ActResponse> GetAsync(string cerp)
        {
            if (string.IsNullOrEmpty(cerp))
                throw new BadRequestException("O cerp é obrigatróio.");

            var result = await _ieCartorioClient.AtoConsultarAtoAsync(cerp);

            if (result == null)
                throw new NotFoundException("Ato não encontrado");

            var orderInput = await _orderService.GetOrderByCerpAsync(cerp);
            var act = orderInput.Atos.FirstOrDefault(x => x.Cerp == cerp);
            var urlAct = act.UrlAct;

            if (string.IsNullOrEmpty(act.UrlAct) && result.Status == "Ato Disponivel")
            {
                try
                {
                    urlAct = await DownloadAsync(cerp);
                }
                catch { }
            }

            act.UrlAct = urlAct;
            result.UrlAct = urlAct;
            await _orderService.UpdateAsync(orderInput.Id, _mapper.Map<Order>(orderInput));

            var resultMapped = _mapper.Map<ActResponse>(result);

            return resultMapped;
        }

        public async Task<List<object>> GetRegistryByActAsync(int actId, string city)
        {
            var list = new List<object>();
            var result = await _ieCartorioClient.FuncaoListarCartorioPorAtoAsync(actId, city);
            if (result == null)
                throw new NotFoundException("Ato não encontrado");

            foreach (var item in result)
                list.Add(new { act = actId, name = item.Nome, registry = item.CodigoServentia, value = item.Valor });

            return list;
        }

        public async Task<object> ViewAsync(string cerp)
        {
            if (string.IsNullOrEmpty(cerp))
                throw new BadRequestException("O cerp é obrigatório.");

            var result = await _ieCartorioClient.AtoVistartAtoAsync(cerp);

            if (result == null)
                throw new NotFoundException("Ato não encontrado");

            return result;
        }
    }
}
