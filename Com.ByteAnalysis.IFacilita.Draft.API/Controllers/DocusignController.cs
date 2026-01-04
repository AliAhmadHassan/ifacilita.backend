using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocusignController : ControllerBase
    {
        public DocusignController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = System.Net.WebUtility.UrlDecode(Request.QueryString.ToString()).Split('&');
            await System.IO.File.WriteAllLinesAsync("get-docusign.log", query);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var query = System.Net.WebUtility.UrlDecode(Request.QueryString.ToString()).Split('&');
            await System.IO.File.WriteAllLinesAsync("post-docusign.log", query);
            return Ok();
        }
    }
}
