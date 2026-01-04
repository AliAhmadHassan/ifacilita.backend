using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IptuDebitController : ControllerBase
    {
        Service.IRequisitionService service;

        public IptuDebitController(Service.IRequisitionService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult Get()
        {
            Model.Requisition requisition = this.service.GetUnprocessed();

            if (requisition == null)
                return NotFound();

            return Ok(requisition);
        }

        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] string id)
        {
            Model.Requisition requisition = this.service.Get(id);

            if (requisition == null)
                return NotFound();

            return Ok(requisition);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Model.Requisition requisition)
        {
            return Ok(this.service.CreateOrUpdate(requisition));
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
                    var content = new StringContent(JsonConvert.SerializeObject(new { requisition.Id, orderId = "", certiticateType = "IptuDebts" }), Encoding.UTF8, "application/json");
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

            return Ok(this.service.CreateOrUpdate(requisition));
        }
    }
}
