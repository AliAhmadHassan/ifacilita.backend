using Com.ByteAnalysis.IFacilita.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionRGIController : ControllerBase
    {
        Service.ITransactionRGIService service;
        private readonly ILogger<TransactionRGIController> _logger;
        Entity.IApplicationSettings applicationSettings;

        public TransactionRGIController(ITransactionRGIService service,
            ILogger<TransactionRGIController> _logger,
            Entity.IApplicationSettings applicationSettings)
        {
            this.service = service;
            this._logger = _logger;
            this.applicationSettings = applicationSettings;
        }

        // GET: api/City
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/City/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        [HttpGet("{id}/eprotocolo")]
        public IActionResult GetEProtocolo(string id)
        {
            Common.IHttpClientFW httpClient = new Common.Impl.HttpClientFW(this.applicationSettings.URLRGIApi);

            var result = httpClient.Get<Newtonsoft.Json.Linq.JObject>(new string[] { id });
            var rpaStatus = (int)result.Value.GetValue("rpaStatus");
            var status = (int)result.Value.GetValue("status");
            
            

            var eprotocolo = result.Value.GetValue("ePropocolo");
            object eProtocolo = null;
            //
            if (eprotocolo.HasValues) { 
                eProtocolo = new
                {
                    registryFees = (decimal)((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("registryFees"),
                    serviceValues = (decimal)((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("serviceValues"),
                    totalValues = (decimal)((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("totalValues"),
                    protocol = ((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("protocol").ToString(),
                    confirmationScreen = ((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("confirmationScreen").ToString(),
                    confirmationURL = ((Newtonsoft.Json.Linq.JObject)eprotocolo).GetValue("confirmationURL").ToString()
                };
            }


            var error = result.Value.GetValue("errors");
            List<object> errors = null;
            if(error.HasValues)
            {
                errors = new List<object>();
                foreach (var er in error)
                {
                    errors.Add(new {
                        code = (int)((Newtonsoft.Json.Linq.JObject)er).GetValue("code"),
                        field = ((Newtonsoft.Json.Linq.JObject)er).GetValue("field").ToString(),
                        message = ((Newtonsoft.Json.Linq.JObject)er).GetValue("message").ToString(),
                        pathImageError = ((Newtonsoft.Json.Linq.JObject)er).GetValue("pathImageError").ToString()
                    });
                }
            }

            var send = new
            {
                status = status,
                rpaStatus,
                eProtocolo,
                errors
            };

            return Ok(send);
        }

        // GET: api/City/5
        [HttpGet("{id}/callback")]
        public IActionResult GetCallback(string id)
        {
            Common.IHttpClientFW httpClient = new Common.Impl.HttpClientFW(this.applicationSettings.URLRGIApi);

            var result = httpClient.Get<Newtonsoft.Json.Linq.JObject>(new string[] { id });

            var eprotocolo = result.Value.GetValue("ePropocolo");
            var status = (int)result.Value.GetValue("status");
            var idTransaction = (int)result.Value.GetValue("idTransaction");

            if (status == 2)
                this.service.Completed(idTransaction);
            return Ok();
        }

        // POST: api/City
        [HttpPost]
        public IActionResult Post([FromBody] Entity.TransactionRGI entity)
        {
            return Ok(this.service.Insert(entity));
        }

        [HttpPost("call-robot")]
        public IActionResult PostCallRobot([FromBody] Entity.TransactionRGI entity)
        {
            _logger.LogInformation("calling robot Entity.TransactionRGI: " + Newtonsoft.Json.JsonConvert.SerializeObject(entity));

            try { 
                this.service.CallRPA(entity);
            }
            catch(Exception err)
            {
                _logger.LogError(err, err.Message);
                return new StatusCodeResult(500);
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity.TransactionRGI entity)
        {
            return Ok(this.service.Update(entity));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }
    }
}
