using Com.ByteAnalysis.IFacilita.ITBISP.Service.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.ITBISP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : Controller
    {
        [HttpGet("transaction")]
        public IActionResult GetTransaction()
        {
            return Ok(new EnumTransactionType().GetMapped());
        }

        [HttpGet("financing")]
        public IActionResult GetFinancing()
        {
            return Ok(new EnumFinancingType().GetMapped());
        }

        [HttpGet("registry")]
        public IActionResult GetRegistry()
        {
            return Ok(new EnumRegistryType().GetMapped());
        }
    }
}
