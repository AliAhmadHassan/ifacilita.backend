using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertidaoDebitoCreditoSPController : ControllerBase
    {
        Service.ICertidaoDebitoCreditoSPService service;

        public CertidaoDebitoCreditoSPController(Service.ICertidaoDebitoCreditoSPService service)
        {
            this.service = service;
        }

        [HttpGet("unProcessed")]
        public ActionResult GetUnProcessed()
        {
            Model.CertidaoDebitoCreditoSP requisition = service.GetUnprocessed();

            if (requisition == null)
                return NotFound();

            return Ok(requisition);
        }

        [HttpGet]
        public IActionResult GetAllRequisition()
        {
            try
            {
                List<Model.CertidaoDebitoCreditoSP> requisitions = service.Get();
                if (requisitions.Count() == 0)
                    return NotFound(new { code = 404, message = "requisitions not found" });

                return Ok(requisitions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            try
            {
                var result = service.Get(id);
                if (result == null)
                    return NotFound(new { message = "Requisition not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Model.CertidaoDebitoCreditoSP certidao)
        {
            return StatusCode(201, this.service.CreateOrUpdate(certidao));
        }

        [HttpPut]
        public ActionResult Put([FromBody] Model.CertidaoDebitoCreditoSP requisition)
        {
            requisition.StatusModified = DateTime.Now;
            var result = service.CreateOrUpdate(requisition);

            if (!string.IsNullOrEmpty(requisition.UrlCallback))
            {
                List<GlobalError> errors = new List<GlobalError>();
                if (requisition.Errors != null)
                    errors = requisition.Errors.ToList();

                try
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var content = new StringContent(JsonConvert.SerializeObject(new { requisition.Id, orderId = "", certiticateType = "TaxDebts" }), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(requisition.UrlCallback, content).Result;
                }
                catch (Exception ex)
                {
                    errors.Add(new GlobalError()
                    {
                        Message = $"Houve uma falha ao tentar atualizar os dados da requisição. " + ex.Message,
                        Code = 0,
                        Field = "Put"
                    });
                }

                requisition.Errors = errors;
                result = service.CreateOrUpdate(requisition);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            try
            {
                service.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
