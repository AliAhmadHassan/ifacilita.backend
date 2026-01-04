using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        [HttpGet("JudicialDistrict")]
        public IActionResult Get()
        {
            return Ok(new Service.Mapper.EnumMapperJudicialDistrict().GetMapped());
        }

        [HttpGet("DocumentType")]
        public IActionResult GetDocumentType()
        {
            return Ok(new[] { new { code = 0, Description = "RG" }, new { code = 1, Description = "RGE" } } );
        }

        [HttpGet("Coverage")]
        public IActionResult GetCoverage()
        {
            return Ok(new[] { new { code = 0, Description = "LAST5YEAR" }, new { code = 1, Description = "LAST10YEAR" } });
        }

        [HttpGet("Expedition")]
        public IActionResult GetExpedition()
        {
            return Ok(new[] { new { code = 0, Description = "DIGITAL" }, new { code = 1, Description = "PAPER" } });
        }

        [HttpGet("PersonType")]
        public IActionResult GetPersonType()
        {
            return Ok(new[] { new { code = 0, Description = "PHYSICAL" }, new { code = 1, Description = "LEGAL" } });
        }
    }
}
