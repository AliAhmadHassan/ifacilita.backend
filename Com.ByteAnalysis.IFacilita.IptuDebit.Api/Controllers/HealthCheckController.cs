using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetFullAsync()
        {
            return await Task.FromResult(Ok());
        }
    }
}
