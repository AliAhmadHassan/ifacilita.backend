using Com.ByteAnalysis.IFacilita.CertiCadSp.Model;
using Com.ByteAnalysis.IFacilita.CertiCadSp.Service;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertidaoCadastraisSpController : ControllerBase
    {
        IRequisitionService service;

        public CertidaoCadastraisSpController(IRequisitionService service)
        {
            this.service = service;
        }

        [HttpGet("unProcessed")]
        public IActionResult GetUnprocessed()
        {
            Requisition requisition = service.GetUnprocessed();

            if (requisition == null)
                return NotFound();

            return Ok(requisition);
        }

        [HttpGet]
        public IActionResult GetAllRequisition()
        {
            try
            {
                List<Requisition> requisitions = service.Get();
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
            requisition.StatusProcess = Status.Waiting;
            return StatusCode(200, this.service.CreateOrUpdate(requisition));
        }

        [HttpPut]
        public ActionResult Put([FromBody] Model.Requisition requisition)
        {
            requisition.StatusModified = DateTime.Now;
            var result = service.CreateOrUpdate(requisition);

            if (!string.IsNullOrEmpty(requisition.UrlCallback))
            {
                List<GlobalError> errors = new List<GlobalError>();
                if (requisition.Errors != null)
                    errors = requisition.Errors.ToList();

                requisition.CallbackResponse = DateTime.Now;
                
                try
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var content = new StringContent(JsonConvert.SerializeObject(new { requisition.Id, orderId = "", certiticateType = "PropertyRegistrationData" }), Encoding.UTF8, "application/json");
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

            return Ok(requisition);
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
