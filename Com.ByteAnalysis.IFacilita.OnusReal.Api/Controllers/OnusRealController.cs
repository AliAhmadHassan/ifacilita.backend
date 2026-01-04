using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Com.ByteAnalysis.IFacilita.OnusReal.Model;
using Com.ByteAnalysis.IFacilita.OnusReal.Service;
using System.Net.Http;
using Newtonsoft.Json;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OnusRealController : ControllerBase
    {
        IRequisitionService service;

        public OnusRealController(IRequisitionService service)
        {
            this.service = service;
        }

        [HttpGet("unProcessed")]
        public IActionResult GetUnprocessed()
        {
            Requisition requisition = service.GetUnprocessed();

            if (requisition == null)
                return NotFound(new { code = 404, message = "not found!" });

            return Ok(requisition);
        }

        [HttpGet]
        public IActionResult GetAllRequisition()
        {
            try
            {
                List<Requisition> requisitions = service.Get();
                if (requisitions.Count() == 0)
                    return NotFound(new { code = 404, message = "not found!" });

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
        public ActionResult Post([FromBody] Requisition requisition)
        {
            try
            {
                var requisitionCurrent = service.Get(requisition.NumCartorio, requisition.NumMatricola);
                if (requisitionCurrent != null)
                {
                    requisition.Protocolo = requisitionCurrent.Protocolo;
                    requisition.Received = DateTime.Now;
                    requisition.Request = DateTime.Now;
                    requisition.s3patch = requisitionCurrent.s3patch;
                    requisition.Status = Common.Enumerable.APIStatus.Success;
                    requisition.CallbackResponse = requisitionCurrent.CallbackResponse;
                    requisition.Expiration = DateTime.Now.AddDays(5);
                    requisition.StatusModified = DateTime.Now;
                    requisition.StatusProcess = Status.Finished;

                    _ = service.CreateOrUpdate(requisition);

                    return Ok(requisition);
                }
                else
                {
                    return Ok(service.CreateOrUpdate(requisition));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody] Model.Requisition requisition)
        {
            var result = this.service.CreateOrUpdate(requisition);

            if (!string.IsNullOrEmpty(requisition.UrlCallback))
            {
                requisition.CallbackResponse = DateTime.Now;
                ExecuteCallback(requisition);
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

        private void ExecuteCallback(Requisition requisition)
        {
            List<GlobalError> errors = new List<GlobalError>();
            if (requisition.Errors != null)
                errors = requisition.Errors.ToList();

            if (!string.IsNullOrEmpty(requisition.UrlCallback))
            {
                try
                {
                    requisition.StatusModified = DateTime.Now;

                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var content = new StringContent(JsonConvert.SerializeObject(new { requisition.Id, orderId = "", certiticateType = "RealOnus" }), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(requisition.UrlCallback, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        requisition.CallbackResponse = DateTime.Now;
                        errors.Add(new GlobalError() { Code= (int)response.StatusCode, Field= "UrlCallback", Message = response.Content.ReadAsStringAsync().Result });
                    }

                }
                catch (Exception ex)
                {
                    requisition.Errors = new List<GlobalError>(){
                    new GlobalError()
                    {
                        Message = $"Houve uma falha ao tentar atualizar os dados da requisição. " + ex.Message,
                        Code = 0,
                        Field="Put"
                    }};
                }
            }

            this.service.CreateOrUpdate(requisition);
        }
    }
}
