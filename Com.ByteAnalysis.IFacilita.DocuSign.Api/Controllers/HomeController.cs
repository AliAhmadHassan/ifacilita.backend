using Microsoft.AspNetCore.Mvc;
using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { code = 200, message = "Seja bem vindo", date = DateTime.Now });
        }
    }
}
