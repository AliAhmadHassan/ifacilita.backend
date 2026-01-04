using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IOrderService _service;
        private const string messageErrorBadRequest = "Houve uma falha no processamento da requisição. Verifique os logs para mais detalhes";

        public CallbackController(IOrderService service)
        {
            _service = service;
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("SearchProtest")]
        public IActionResult SearchProtest([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("DefectsDefined")]
        public IActionResult DefectsDefined([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("TaxDebts")]
        public IActionResult TaxDebts([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("IptuDebts")]
        public IActionResult IptuDebts([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("PropertyRegistrationData")]
        public IActionResult PropertyRegistrationData([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost("RealOnus")]
        public IActionResult RealOnus([FromBody] ResponseCallbackDto response)
        {
            try
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(response));
                if (!_service.ProcessCallback(response.Id, response.CertiticateType)) return BadRequest(messageErrorBadRequest);
                return Ok();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("[ERROR] - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
