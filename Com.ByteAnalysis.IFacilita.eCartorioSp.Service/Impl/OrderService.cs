using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Repository;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        private readonly IOrderRepository _repository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IClientApiSearchProtest _clientApiSearchProtest;
        private readonly IDefectsDefinedClientApi _defectsDefinedClientApi;
        private readonly IClientTaxDebtsApi _clientTaxDebtsApi;
        private readonly IClientIptuDebtsApi _clientIptuDebtsApi;
        private readonly IClientRealOnusApi _clientRealOnusApi;
        private readonly IClientPropertyRegistrationDataApi _clientPropertyRegistrationDataApi;

        public OrderService(
            IMapper mapper,
            IConfiguration configuration,
            ICertificateRepository certificateRepository,
            IOrderRepository repository,
            IClientApiSearchProtest clientApiSearchProtest,
            IDefectsDefinedClientApi defectsDefinedClientApi,
            IClientTaxDebtsApi clientTaxDebtsApi,
            IClientIptuDebtsApi clientIptuDebtsApi,
            IClientRealOnusApi clientRealOnusApi,
            IClientPropertyRegistrationDataApi clientPropertyRegistrationDataApi
            )
        {
            _configuration = configuration;
            _mapper = mapper;

            _repository = repository;
            _certificateRepository = certificateRepository;
            _clientApiSearchProtest = clientApiSearchProtest;
            _defectsDefinedClientApi = defectsDefinedClientApi;
            _clientTaxDebtsApi = clientTaxDebtsApi;
            _clientIptuDebtsApi = clientIptuDebtsApi;
            _clientRealOnusApi = clientRealOnusApi;
            _clientPropertyRegistrationDataApi = clientPropertyRegistrationDataApi;
        }

        public OrderEntity Create(OrderEntity entry) => _repository.Create(entry);

        public OrderEntity Create(OrderDto order)
        {
            OrderEntity orderCreated = new OrderEntity();
            var certificates = _certificateRepository.Get();
            var certificatesActives = _configuration["Apis:CertiticatesActives"];
            var urlCurrent = string.Empty;

            PrepareMessageLogProcessCallback($"Order : {JsonConvert.SerializeObject(order)}", CertiticateType.IptuDebts, "Iniciando processo de requisição");

            foreach (var cert in certificates)
            {
                if (!certificatesActives.Contains(cert.CertiticateType.ToString()))
                    continue;

                try
                {
                    switch (cert.CertiticateType)
                    {
                        case CertiticateType.SearchProtest:
                            #region SearchProtest
                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.SearchProtest, "Iniciando a requisição **SearchProtest** - Pesquisa de Protestos");

                            urlCurrent = _configuration["Apis:SearchProtest:UrlApi"];
                            var additionalSearchProtest = order.Registry.FirstOrDefault(x => x.CertiticateType == CertiticateType.SearchProtest);

                            CertificateSearchProtestResponse body = new CertificateSearchProtestResponse()
                            {
                                AllRegistry = true,
                                Coverage = Coverage.LAST10YEAR,
                                Created = DateTime.Now,
                                DocumentComplementary = order.DataSearch[0].Rg,
                                DocumentPrincipal = !string.IsNullOrEmpty(order.DataSearch[0].Cnpj) ? order.DataSearch[0].Cnpj : order.DataSearch[0].Cpf,
                                DocumentType = DocumentType.RG,
                                Expedition = Expedition.DIGITAL,
                                FullName = order.DataSearch[0].Name,
                                JuridicalDistrictCode = 3550308,
                                Pending = true,
                                OrderNumber = null,
                                PersonType = !string.IsNullOrEmpty(order.DataSearch[0].Cnpj) ? PersonType.LEGAL : PersonType.PHYSICAL,
                                StatusDownloadCertificates = "Unproccessed",
                                Updated = DateTime.Now,
                                UrlCallback = _configuration["Apis:SearchProtest:UrlCallback"],
                                Approved = Convert.ToBoolean(additionalSearchProtest.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("Approved")).Value)
                            };

                            var response = _clientApiSearchProtest.Post(body);
                            if (response == null)
                            {
                                PrepareMessageLogProcessCallback($"SearchProtest: '{JsonConvert.SerializeObject(body)}'", CertiticateType.SearchProtest, "FALHA na Requisição **SearchProtest** - Pesquisa de Protestos concluída com sucesso");
                                continue;
                            }

                            var certMapped = _mapper.Map<CertificateSearchProtestEntity>(response);
                            certMapped.CertiticateType = CertiticateType.SearchProtest;
                            certMapped.DescriptionCertificateType = cert.Description;

                            orderCreated.SearchProtest = certMapped;
                            orderCreated.Value += cert.Value;
                            PrepareMessageLogProcessCallback($"SearchProtest: '{certMapped.Id}'", CertiticateType.SearchProtest, "Finalizando a Requisição **SearchProtest** - Pesquisa de Protestos concluída com sucesso");

                            break;
                        #endregion
                        case CertiticateType.DefectsDefined:
                            #region DefectsDefined
                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.DefectsDefined, "Iniciando a requisição  **DefectsDefined** - Defeitos Ajuizados");
                            urlCurrent = _configuration["Apis:DefectsDefined:UrlApi"];

                            CertificateDefectsDefinedEntityRequestResponse bodyRequest = new CertificateDefectsDefinedEntityRequestResponse()
                            {
                                Cpf = order.DataSearch[0].Cpf,
                                Email = order.DataSearch[0].Email,
                                FullName = order.DataSearch[0].Name,
                                GenderType = order.DataSearch[0].Gender,
                                PersonType = !string.IsNullOrEmpty(order.DataSearch[0].Cnpj) ? PersonType.LEGAL : PersonType.PHYSICAL,
                                Pending = true,
                                Rg = order.DataSearch[0].Rg,
                                UrlCallback = _configuration["Apis:DefectsDefined:UrlCallback"],
                                ModelCode = 52,
                            };

                            var responseDefectsDefined = _defectsDefinedClientApi.Post(bodyRequest);
                            if (responseDefectsDefined == null)
                            {
                                PrepareMessageLogProcessCallback($"DefectsDefined: '{JsonConvert.SerializeObject(bodyRequest)}'", CertiticateType.DefectsDefined, "Finalizado a requisição  **DefectsDefined** - Defeitos Ajuizados");
                                continue;
                            }

                            var certMappedDefectsDefined = _mapper.Map<CertificateDefectsDefinedEntity>(responseDefectsDefined);
                            certMappedDefectsDefined.CertiticateType = CertiticateType.DefectsDefined;
                            certMappedDefectsDefined.DescriptionCertificateType = cert.Description;

                            orderCreated.DefectsDefined = certMappedDefectsDefined;
                            orderCreated.Value += cert.Value;

                            PrepareMessageLogProcessCallback($"DefectsDefined: '{certMappedDefectsDefined.Id}'", CertiticateType.DefectsDefined, "Finalizado a requisição  **DefectsDefined** - Defeitos Ajuizados");

                            break;

                        #endregion
                        case CertiticateType.TaxDebts:
                            #region TaxDebts

                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.TaxDebts, "Iniciando a requisição de **TaxDebts** - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União");

                            urlCurrent = _configuration["Apis:TaxDebts:UrlApi"];

                            var additionalTaxDebts = order.Registry.FirstOrDefault(x => x.CertiticateType == CertiticateType.TaxDebts);

                            TaxDebtsRequestResponse bodyTaxDebts = new TaxDebtsRequestResponse()
                            {
                                CpfCnpj = !string.IsNullOrEmpty(order.DataSearch[0].Cnpj) ? order.DataSearch[0].Cnpj : order.DataSearch[0].Cpf,
                                UrlCallback = _configuration["Apis:TaxDebts:UrlCallback"],
                                DateInsert = DateTime.Now,
                                IdUserIfacilita = Convert.ToInt32(additionalTaxDebts.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("IdUserIfacilita")).Value),
                                Nome = order.DataSearch[0].Name,
                                PessoaFisica = !string.IsNullOrEmpty(order.DataSearch[0].Cnpj) ? false : true,
                                StatusModified = DateTime.Now,
                                StatusProcess = ApiProcessStatus.Waiting
                            };

                            var responseTaxDebts = _clientTaxDebtsApi.Post(bodyTaxDebts);
                            if (responseTaxDebts == null)
                            {
                                PrepareMessageLogProcessCallback($"TaxDebts: '{JsonConvert.SerializeObject(bodyTaxDebts)}'", CertiticateType.TaxDebts, "FALHA na requisição de **TaxDebts** - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União");
                                continue;
                            }

                            var certMappedTaxDebts = _mapper.Map<CertificateTaxDebtsEntity>(responseTaxDebts);
                            certMappedTaxDebts.CertiticateType = CertiticateType.TaxDebts;
                            certMappedTaxDebts.DescriptionCertificateType = cert.Description;

                            orderCreated.TaxDebts = certMappedTaxDebts;
                            orderCreated.Value += cert.Value;

                            PrepareMessageLogProcessCallback($"TaxDebts: '{certMappedTaxDebts.Id}'", CertiticateType.TaxDebts, "Finalizado a requisição de **TaxDebts** - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União");
                            break;
                        #endregion
                        case CertiticateType.IptuDebts:
                            #region IptuDebts

                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.IptuDebts, "Iniciando a requisição  **IptuDebts** - Débitos do IPTU");

                            urlCurrent = _configuration["Apis:IptuDebts:UrlApi"];

                            IptuDebtsRequestResponse bodyIptuDebts = new IptuDebtsRequestResponse()
                            {
                                UrlCallback = _configuration["Apis:IptuDebts:UrlCallback"],
                                StatusProcess = ApiProcessStatus.Waiting,
                                SQL = order.PropertyDetails.Sql,
                                IdTransaction = order.PropertyDetails.IdTransaction,
                                Expiration = DateTime.Now.AddMonths(3).AddDays(-1),
                                Request = DateTime.Now,
                                Received = DateTime.Now,
                                StatusModified = DateTime.Now
                            };

                            var responseIptuDebts = _clientIptuDebtsApi.Post(bodyIptuDebts);
                            if (responseIptuDebts == null)
                            {
                                PrepareMessageLogProcessCallback($"IptuDebts : {JsonConvert.SerializeObject(bodyIptuDebts)}", CertiticateType.IptuDebts, "FALHA na requisição  **IptuDebts** - Débitos do IPTU");
                                continue;
                            }

                            var certMapperdIptuDebts = _mapper.Map<CertificateIptuDebtsEntity>(responseIptuDebts);
                            certMapperdIptuDebts.CertiticateType = CertiticateType.IptuDebts;
                            certMapperdIptuDebts.DescriptionCertificateType = cert.Description;

                            orderCreated.IptuDebts = certMapperdIptuDebts;
                            orderCreated.Value += cert.Value;

                            PrepareMessageLogProcessCallback($"IptuDebts: '{certMapperdIptuDebts.Id}'", CertiticateType.IptuDebts, "Finalizado a requisição  **IptuDebts** - Débitos do IPTU");

                            break;
                        #endregion
                        case CertiticateType.PropertyRegistrationData:
                            #region PropertyRegistrationData
                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.PropertyRegistrationData, "Iniciando a requisição  **PropertyRegistrationData** - Dados Cadastrais do Imóvel");

                            urlCurrent = _configuration["Apis:PropertyRegistrationData:UrlApi"];
                            var additionalRegistryPropertyRegistrationData = order.Registry.FirstOrDefault(x => x.CertiticateType == CertiticateType.PropertyRegistrationData);

                            PropertyRegistrationDataRequestResponse bodyPropertyRegistrationData = new PropertyRegistrationDataRequestResponse()
                            {
                                CpfCnPj = additionalRegistryPropertyRegistrationData.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("Login")).Value,
                                UrlCallback = _configuration["Apis:PropertyRegistrationData:UrlCallback"],
                                Sql = order.PropertyDetails.Sql,
                                dateDoc = additionalRegistryPropertyRegistrationData.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("DateDoc")).Value,
                                Password = additionalRegistryPropertyRegistrationData.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("Password")).Value,
                            };

                            var responsePropertyRegistrationData = _clientPropertyRegistrationDataApi.Post(bodyPropertyRegistrationData);

                            if (responsePropertyRegistrationData == null)
                            {
                                PrepareMessageLogProcessCallback($"PropertyRegistrationData : {JsonConvert.SerializeObject(bodyPropertyRegistrationData)}", CertiticateType.PropertyRegistrationData, "FALHA na requisição  **PropertyRegistrationData** - Dados Cadastrais do Imóvel");
                                continue;
                            }

                            var certMappedPropertyRegistrationData = _mapper.Map<CertificatePropertyRegistrationDataEntity>(responsePropertyRegistrationData);
                            certMappedPropertyRegistrationData.CertiticateType = CertiticateType.PropertyRegistrationData;
                            certMappedPropertyRegistrationData.DescriptionCertificateType = cert.Description;

                            orderCreated.PropertyRegistrationData = certMappedPropertyRegistrationData;
                            orderCreated.Value += cert.Value;

                            PrepareMessageLogProcessCallback($"PropertyRegistrationData: '{certMappedPropertyRegistrationData.Id}'", CertiticateType.PropertyRegistrationData, "Finalizando a requisição  **PropertyRegistrationData** - Dados Cadastrais do Imóvel");
                            break;

                        #endregion
                        case CertiticateType.RealOnus:
                            #region RealOnus
                            PrepareMessageLogProcessCallback($"Order: ''", CertiticateType.RealOnus, "Iniciando a requisição  **RealOnus** - Ônus Reais");

                            urlCurrent = _configuration["Apis:RealOnus:UrlApi"];
                            var additionalRegistry = order.Registry.FirstOrDefault(x => x.CertiticateType == CertiticateType.RealOnus);

                            RealOnusRequestResponse bodyRealOnus = new RealOnusRequestResponse()
                            {
                                CallbackResponse = DateTime.Now,
                                Expiration = DateTime.Now.AddDays(2),
                                IdUser = Convert.ToInt32(additionalRegistry.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("IdUser")).Value),
                                NumCartorio = Convert.ToInt32(additionalRegistry.Registry),
                                NumMatricola = order.PropertyDetails.Registry,
                                Protocolo = additionalRegistry.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("Protocolo")).Value,
                                Received = DateTime.Now,
                                Request = DateTime.Now,
                                StatusProcess = ApiProcessStatus.Waiting,
                                StatusModified = DateTime.Now,
                                UrlCallback = _configuration["Apis:RealOnus:UrlCallback"],
                                DocumentVisualization = Convert.ToBoolean(additionalRegistry.AdditionalInfo.FirstOrDefault(x => x.Key.Equals("DocumentVisualization")).Value)
                            };

                            var responseRealOnus = _clientRealOnusApi.Post(bodyRealOnus);
                            if (responseRealOnus == null)
                            {
                                PrepareMessageLogProcessCallback($"RealOnus: '{JsonConvert.SerializeObject(bodyRealOnus)}'", CertiticateType.RealOnus, "FALHA na requisição  **RealOnus** - Ônus Reais");
                                continue;
                            }

                            var certMappedRealOnus = _mapper.Map<CertificateRealOnusEntity>(responseRealOnus);
                            certMappedRealOnus.CertiticateType = CertiticateType.RealOnus;
                            certMappedRealOnus.DescriptionCertificateType = cert.Description;

                            orderCreated.RealOnus = certMappedRealOnus;
                            orderCreated.Value += cert.Value;

                            PrepareMessageLogProcessCallback($"RealOnus: '{certMappedRealOnus.Id}'", CertiticateType.RealOnus, "Finalizado a requisição  **RealOnus** - Ônus Reais");
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
                catch (AggregateException aEx)
                {
                    OutputLogs(new[] {
                            $"Momento do Erro: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                            $"--------------------------------------------",
                            $"Url: {urlCurrent}",
                            $"Certificado: {cert.CertiticateType} - {cert.Description}",
                            $"Erro Ocorrido: {aEx.Message}",
                            $"Payload: {JsonConvert.SerializeObject(order)}",
                            $"*********************************************************************\n"
                        });
                }
                catch (Exception ex)
                {
                    OutputLogs(new[] {
                            $"Momento do Erro: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                            $"--------------------------------------------",
                            $"Url: {urlCurrent}",
                            $"Erro Ocorrido: {ex.Message}",
                            $"Payload: {JsonConvert.SerializeObject(order)}",
                            $"*********************************************************************\n"
                        });
                }
            }

            orderCreated.Created = DateTime.Now;
            orderCreated.Updated = DateTime.Now;

            orderCreated = _repository.Create(orderCreated);

            return orderCreated;
        }

        private void OutputLogs(IEnumerable<string> lines)
        {
            if (!System.IO.Directory.Exists("Logs"))
                System.IO.Directory.CreateDirectory("Logs");

            try
            {
                System.IO.File.AppendAllLines($"Logs/eCartorioSP-API-{DateTime.Now.ToString("dd-MM-yyyy-HH")}.log", lines);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"Erro ao tentar gravar arquivo de logs. A mensagem do sistema foi: {ex.Message}, Pilha: {ex.StackTrace}");
            }
        }

        public OrderEntity Create(OrderCertiticateDto order)
        {
            throw new NotImplementedException();
        }

        public List<OrderEntity> Get() => _repository.Get();

        public OrderEntity Get(string id) => _repository.Get(id);

        public void Remove(OrderEntity entry) => _repository.Remove(entry);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, OrderEntity entry)
        {
            _repository.Update(id, entry);
        }

        public OrderEntity Get(string id, CertiticateType type) => _repository.Get(id, type);

        private bool PrepareMessageLogProcessCallback(string id, CertiticateType type, string msg = "A consulta no servidor não retornou resultados")
        {
            var messagageLog = new[] {
                $"Data/Hora: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                $"--------------------------------------------",
                $"Tipo Certificado: {type}",
                $"Id: {id}",
                $"{msg}",
                $"*********************************************************************\n"
            };

            System.Console.Out.WriteLine($"[ {DateTime.Now} ] - {msg}");

            OutputLogs(messagageLog);

            return false;
        }

        public bool ProcessCallback(string id, CertiticateType type)
        {
            var order = Get(id, type);
            if (order == null)
            {
                PrepareMessageLogProcessCallback(id, type, "O Pedido não foi encontrado");
                return false;
            }

            PrepareMessageLogProcessCallback(id, type, "Pedido: " + JsonConvert.SerializeObject(order));

            switch (type)
            {
                case CertiticateType.SearchProtest:
                    var response = _clientApiSearchProtest.Get(id);

                    if (response == null) return PrepareMessageLogProcessCallback(id, type);

                    //PrepareMessageLogProcessCallback(id, type, JsonConvert.SerializeObject(response));

                    var responseMapper = _mapper.Map<CertificateSearchProtestEntity>(response);

                    responseMapper.CertiticateType = type;
                    responseMapper.DescriptionCertificateType = type.ToString();

                    //PrepareMessageLogProcessCallback(id, type, "Objeto Mapeado: " + JsonConvert.SerializeObject(responseMapper));

                    order.SearchProtest = responseMapper;

                    _repository.Update(order.Id, order);
                    return true;

                case CertiticateType.DefectsDefined:
                    var responseDefectsDefined = _defectsDefinedClientApi.Get(id);
                    if (responseDefectsDefined == null) return PrepareMessageLogProcessCallback(id, type);

                    var responseMapperDefectsDefined = _mapper.Map<CertificateDefectsDefinedEntity>(responseDefectsDefined);
                    responseMapperDefectsDefined.CertiticateType = type;
                    responseMapperDefectsDefined.DescriptionCertificateType = type.ToString();

                    order.DefectsDefined = responseMapperDefectsDefined;
                    _repository.Update(order.Id, order);
                    return true;

                case CertiticateType.TaxDebts:
                    var responseTaxDebts = _clientTaxDebtsApi.Get(id);

                    if (responseTaxDebts == null) return PrepareMessageLogProcessCallback(id, type);

                    var responseMapperTaxDebts = _mapper.Map<CertificateTaxDebtsEntity>(responseTaxDebts);
                    responseMapperTaxDebts.CertiticateType = type;
                    responseMapperTaxDebts.DescriptionCertificateType = type.ToString();

                    order.TaxDebts = responseMapperTaxDebts;
                    _repository.Update(order.Id, order);
                    return true;

                case CertiticateType.IptuDebts:

                    var responseIptuDebts = _clientIptuDebtsApi.Get(id);
                    if (responseIptuDebts == null) return PrepareMessageLogProcessCallback(id, type);
                    var responseMapperIptuDebts = _mapper.Map<CertificateIptuDebtsEntity>(responseIptuDebts);
                    responseMapperIptuDebts.CertiticateType = type;
                    responseMapperIptuDebts.DescriptionCertificateType = type.ToString();

                    order.IptuDebts = responseMapperIptuDebts;
                    _repository.Update(order.Id, order);
                    return true;

                case CertiticateType.PropertyRegistrationData:
                    var responsePropertyRegistrationData = _clientPropertyRegistrationDataApi.Get(id);
                    if (responsePropertyRegistrationData == null) return PrepareMessageLogProcessCallback(id, type);
                    var responseMapperPropertyRegistrationData = _mapper.Map<CertificatePropertyRegistrationDataEntity>(responsePropertyRegistrationData);

                    responseMapperPropertyRegistrationData.CertiticateType = type;
                    responseMapperPropertyRegistrationData.DescriptionCertificateType = type.ToString();

                    order.PropertyRegistrationData = responseMapperPropertyRegistrationData;
                    _repository.Update(order.Id, order);
                    return true;

                case CertiticateType.RealOnus:
                    var responseRealOnus = _clientRealOnusApi.Get(id);
                    if (responseRealOnus == null) return PrepareMessageLogProcessCallback(id, type);
                    var responseMapperRealOnus = _mapper.Map<CertificateRealOnusEntity>(responseRealOnus);
                    responseMapperRealOnus.CertiticateType = type;
                    responseMapperRealOnus.DescriptionCertificateType = type.ToString();

                    order.RealOnus = responseMapperRealOnus;
                    _repository.Update(order.Id, order);
                    return true;
            }

            return false;
        }
    }
}
