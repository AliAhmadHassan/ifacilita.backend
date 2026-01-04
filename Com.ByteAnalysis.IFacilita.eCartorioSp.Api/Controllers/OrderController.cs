using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _service;
        private readonly IClientApiSearchProtest _clientApiSearchProtest;
        private readonly IDefectsDefinedClientApi _defectsDefinedClientApi;
        private readonly IClientTaxDebtsApi _clientTaxDebtsApi;
        private readonly IClientIptuDebtsApi _clientIptuDebtsApi;
        private readonly IClientRealOnusApi _clientRealOnusApi;
        private readonly IClientPropertyRegistrationDataApi _clientPropertyRegistrationDataApi;

        public OrderController(
            IMapper mapper,
            IOrderService service,
            IClientApiSearchProtest clientApiSearchProtest,
            IDefectsDefinedClientApi defectsDefinedClientApi,
            IClientTaxDebtsApi clientTaxDebtsApi,
            IClientIptuDebtsApi clientIptuDebtsApi,
            IClientRealOnusApi clientRealOnusApi,
            IClientPropertyRegistrationDataApi clientPropertyRegistrationDataApi)
        {
            _mapper = mapper;
            _service = service;
            _clientApiSearchProtest = clientApiSearchProtest;
            _defectsDefinedClientApi = defectsDefinedClientApi;
            _clientTaxDebtsApi = clientTaxDebtsApi;
            _clientIptuDebtsApi = clientIptuDebtsApi;
            _clientRealOnusApi = clientRealOnusApi;
            _clientPropertyRegistrationDataApi = clientPropertyRegistrationDataApi;
        }

        [ProducesResponseType(200, Type = typeof(OrderEntity))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            try
            {
                var resultOrder = _service.Get(id);
                if (resultOrder == null)
                    return NotFound(new { code = 404, message = "order not found!" });

                CertificateMapperService certificateMapperService = new CertificateMapperService();
                CertificateMapped certMapped = new CertificateMapped();

                //**SearchProtest** - Pesquisa de Protestos
                if (resultOrder.SearchProtest != null)
                {
                    var searchProtest = _clientApiSearchProtest.Get(resultOrder.SearchProtest.Id);
                    if (searchProtest != null)
                    {
                        resultOrder.SearchProtest = _mapper.Map<CertificateSearchProtestEntity>(searchProtest);
                        certMapped = certificateMapperService.Get(CertiticateType.SearchProtest);
                        resultOrder.SearchProtest.CertiticateType = certMapped.CertiticateType;
                        resultOrder.SearchProtest.DescriptionCertificateType = certMapped.Description;
                    }
                }

                //**DefectsDefined** - Defeitos Ajuizados
                if (resultOrder.DefectsDefined != null)
                {
                    var defectsDefined = _defectsDefinedClientApi.Get(resultOrder.DefectsDefined.Id);
                    if (defectsDefined != null)
                    {
                        resultOrder.DefectsDefined = _mapper.Map<CertificateDefectsDefinedEntity>(defectsDefined);
                        certMapped = certificateMapperService.Get(CertiticateType.DefectsDefined);
                        resultOrder.DefectsDefined.CertiticateType = certMapped.CertiticateType;
                        resultOrder.DefectsDefined.DescriptionCertificateType = certMapped.Description;
                    }
                }

                //**TaxDebts** - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União
                if (resultOrder.TaxDebts != null)
                {
                    var taxDebts = _clientTaxDebtsApi.Get(resultOrder.TaxDebts.Id);
                    if (taxDebts != null)
                    {
                        resultOrder.TaxDebts = _mapper.Map<CertificateTaxDebtsEntity>(taxDebts);
                        certMapped = certificateMapperService.Get(CertiticateType.TaxDebts);
                        resultOrder.TaxDebts.CertiticateType = certMapped.CertiticateType;
                        resultOrder.TaxDebts.DescriptionCertificateType = certMapped.Description;
                    }

                }

                //**IptuDebts** - Débitos do IPTU
                if (resultOrder.IptuDebts != null)
                {
                    var iptuDebts = _clientIptuDebtsApi.Get(resultOrder.IptuDebts.Id);
                    if (iptuDebts != null)
                    {
                        resultOrder.IptuDebts = _mapper.Map<CertificateIptuDebtsEntity>(iptuDebts);
                        certMapped = certificateMapperService.Get(CertiticateType.IptuDebts);
                        resultOrder.IptuDebts.CertiticateType = certMapped.CertiticateType;
                        resultOrder.IptuDebts.DescriptionCertificateType = certMapped.Description;
                    }
                }

                //**RealOnus** - Ônus Reais
                if (resultOrder.RealOnus != null)
                {
                    var realOnus = _clientRealOnusApi.Get(resultOrder.RealOnus.Id);
                    if (realOnus != null)
                    {
                        resultOrder.RealOnus = _mapper.Map<CertificateRealOnusEntity>(realOnus);
                        certMapped = certificateMapperService.Get(CertiticateType.RealOnus);
                        resultOrder.RealOnus.CertiticateType = certMapped.CertiticateType;
                        resultOrder.RealOnus.DescriptionCertificateType = certMapped.Description;
                    }
                }

                // **PropertyRegistrationData** - Dados Cadastrais do Imóvel
                if (resultOrder.PropertyRegistrationData != null)
                {
                    var propertyRegistrationData = _clientPropertyRegistrationDataApi.Get(resultOrder.PropertyRegistrationData.Id);
                    if (propertyRegistrationData != null)
                    {
                        resultOrder.PropertyRegistrationData = _mapper.Map<CertificatePropertyRegistrationDataEntity>(propertyRegistrationData);
                        certMapped = certificateMapperService.Get(CertiticateType.PropertyRegistrationData);
                        resultOrder.PropertyRegistrationData.CertiticateType = certMapped.CertiticateType;
                        resultOrder.PropertyRegistrationData.DescriptionCertificateType = certMapped.Description;
                    }
                }

                _service.Update(resultOrder.Id, resultOrder);

                return Ok(resultOrder);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - Order->Get: " + ex.Message);
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        [ProducesResponseType(200, Type = typeof(OrderEntity))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet()]
        public IActionResult Get()
        {
            var result = _service.Get();
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(OrderEntity))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("all")]
        public IActionResult Post([FromBody] OrderDto order)
        {
            var result = _service.Create(order);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(OrderResponseDto))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("certificate")]
        public IActionResult PostCertiticate([FromBody] OrderCertiticateDto order)
        {
            return Ok(new OrderResponseDto());
        }


    }
}
