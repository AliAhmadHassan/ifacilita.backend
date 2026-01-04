using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightOwnershipController : ControllerBase
    {
        private readonly IService _service;

        public LightOwnershipController(IService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            try
            {
                var result = _service.Get(id);
                if (result == null)
                    return NotFound(new { message = "not found" });

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
                    return NotFound(new { message = "not found" });

                return Ok(result.ToList().Skip(page * 10).Take(count));

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OwnershipModel cri)
        {
            try
            {
                var result = _service.Create(cri);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] OwnershipModel cri)
        {
            try
            {
                _service.Update(cri.Id, cri);

                if (!string.IsNullOrEmpty(cri.UrlCallback))
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var response = client.GetAsync(string.Format(cri.UrlCallback, cri.Id)).Result;

                    cri.UrlCallbackResponse += $"O servidor {cri.UrlCallback} de callback retornou o status: {response.StatusCode}, mensagem: { response.Content.ReadAsStringAsync().Result}";
                    _service.Update(cri.Id, cri);
                }

                return Ok(cri);
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
