using Com.ByteAnalysis.IFacilita.LigthOwnership.Service;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        [HttpGet("uf")]
        public IActionResult GetUf()
        {
            return Ok(new EnumUfType().GetMapped());
        }

        [HttpGet("country")]
        public IActionResult GetCountry()
        {
            return Ok(new EnumCountryType().GetMapped());
        }

        [HttpGet("documenttype")]
        public IActionResult GetDocumentType()
        {
            return Ok(new EnumDocumentType().GetMapped());
        }
    }
}
