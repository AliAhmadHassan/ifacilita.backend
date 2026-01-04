using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.RGI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RGIController : ControllerBase
    {
        Service.IRequisitionService service;
        private readonly ILogger<RGIController> _logger;
        Common.IHttpClientFW httpClientFW;

        public RGIController(Service.IRequisitionService service,
            ILogger<RGIController> _logger)
        {
            this.service = service;
            this._logger = _logger;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = service.Get();
            if (result == null || result.Count() == 0)
                return NotFound(new { code = 404, message="Rgi not found" });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            this._logger.LogInformation($"HttpGet({id})");
            return Ok(this.service.GetById(id));
        }

        [HttpGet("to-proccess")]
        public ActionResult GetToProccess()
        {
            this._logger.LogInformation($"HttpGet(to-proccess)");
            Model.Requisition requisition = this.service.GetUnprocessed();

            if (requisition == null)
            {
                return NotFound();
                this._logger.LogInformation($"Not Found");
            }

            this._logger.LogInformation("HttpGet(to-proccess) Model.Requisition: " + Newtonsoft.Json.JsonConvert.SerializeObject(requisition));

            return Ok(requisition);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Model.Requisition requisition)
        {
            this._logger.LogInformation("HttpPost Model.Requisition: " + Newtonsoft.Json.JsonConvert.SerializeObject(requisition));
            requisition.RpaStatus = Model.Status.Waiting;
            requisition.StatusModified = DateTime.Now;
            requisition.Status = Common.Enumerable.APIStatus.Pending;
            return Ok(this.service.CreateOrUpdate(requisition));
        }

        [HttpPut]
        public ActionResult Put([FromBody] Model.Requisition requisition)
        {
            this._logger.LogInformation("HttpPut Model.Requisition: " + Newtonsoft.Json.JsonConvert.SerializeObject(requisition));


            try
            {
                this._logger.LogInformation("Request Callback to Core");
                this.httpClientFW = new Common.Impl.HttpClientFW(string.Format(requisition.UrlCallback, requisition.Id));
                var result = httpClientFW.Get<object>();
                if (result.IsSuccessfully)
                {
                    requisition.RpaStatus = Model.Status.Delivered;
                    this._logger.LogInformation("Callback successfuly");
                }
                else
                {
                    requisition.RpaStatus = Model.Status.ErrorOnCallback;
                    this._logger.LogError("Callback Failed Error: " + result.ActionResult);
                }
                requisition = this.service.CreateOrUpdate(requisition);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
            }



            return Ok(requisition);
        }
    }
}
