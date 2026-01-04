using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using Com.ByteAnalysis.IFacilita.ITBISP.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.ITBISP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItbiSpController : ControllerBase
    {
        IRequisitionService service;

        public ItbiSpController(IRequisitionService service)
        {
            this.service = service;
        }

        [HttpGet("unProcessed")]
        public ActionResult GetUnProcessed()
        {
            RequisitionItbiModel requisition = service.GetUnprocessed();

            if (requisition == null)
                return NotFound();

            return Ok(requisition);
        }

        [HttpGet]
        public IActionResult GetAllRequisition()
        {
            try
            {
                List<RequisitionItbiModel> requisitions = service.Get();
                if (requisitions == null || requisitions.Count == 0)
                    return NotFound(new { code = 404, message = "Not Found" });

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
                    return NotFound(new { message = "RequisitionItbiModel not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] RequisitionItbiModel requisition)
        {
            try
            {
                var result = this.service.CreateOrUpdate(requisition);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody] RequisitionItbiModel requisition)
        {
            try
            {
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
                        var response = client.GetAsync(requisition.UrlCallback).Result;

                        requisition.ResponseUrlCallback = $"O servidor {requisition.UrlCallback} de callback retornou o status: {response.StatusCode}, mensagem: {response.Content.ReadAsStringAsync().Result}";
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
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
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
