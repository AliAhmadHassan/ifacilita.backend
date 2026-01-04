using Com.ByteAnalysis.IFacilita.eCartorioSp.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Api.Controllers
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

        [ProducesResponseType(200, Type = typeof(Entity.CertificateEntity[]))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet("all")]
        public IActionResult Get([FromQuery] string city)
        {
            var result = _service.Get();
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(Entity.CertificateEntity[]))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {
            var result = _service.Get(id);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(Entity.CertificateEntity))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPost]
        public IActionResult Post([FromBody] Entity.CertificateEntity certificate)
        {
            var result = _service.Create(certificate);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(Entity.CertificateEntity))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [HttpPut]
        public IActionResult Put([FromBody] Entity.CertificateEntity certificate)
        {
            _service.Update(certificate.Id, certificate);
            return Ok(certificate);
        }
    }
}
