using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using Com.ByteAnalysis.IFacilita.CertificateESajSp.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _service;

        public CertificateController(ICertificateService service)
        {
            _service = service;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            try
            {
                var result = _service.Get();
                if (result == null)
                    return NotFound(new { message = "Certificate not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            try
            {
                var result = _service.Get(id);
                if (result == null)
                    return NotFound(new { message = "Certificate not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        public IActionResult GetPendings([FromQuery] int page, [FromQuery] int count)
        {
            try
            {
                _ = page > 0 ? page-- : page;
                var result = _service.GetPendings();

                if (result == null || result.Count() == 0)
                    return NotFound(new { message = "Certificate not found" });

                return Ok(result.ToList().Skip(page * 10).Take(count));

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrent([FromQuery] string document, [FromQuery] DateTime date)
        {
            try
            {
                var result = _service.Get(document, date);

                if (result == null)
                    return NotFound(new { message = "Certificate not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ResumeOrderModel input)
        {
            try
            {
                var result = _service.Create(input);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("callback")]
        public IActionResult PostCallback([FromBody] ResumeOrderModel input)
        {
            try
            {
                return Ok(input);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] ResumeOrderModel input)
        {
            try
            {
                _service.Update(input.Id, input);

                if (!string.IsNullOrEmpty(input.UrlCallback))
                {
                    try
                    {
                        using var clientHandler = new HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                        using var client = new HttpClient(clientHandler);
                        var content = new StringContent(JsonConvert.SerializeObject(new { input.Id, orderId = "", certiticateType = "DefectsDefined" }), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(input.UrlCallback, content).Result;

                        input.UrlCallbackResponse = $"O servidor {input.UrlCallback} de callback retornou o status: {response.StatusCode}, mensagem: {response.Content.ReadAsStringAsync().Result}";
                    }
                    catch (Exception ex)
                    {
                        input.UrlCallbackResponse += ex.Message;
                    }
                    _service.Update(input.Id, input);
                }

                return Ok(input);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            try
            {
                _service.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
