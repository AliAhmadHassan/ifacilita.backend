using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandpageController : ControllerBase
    {
        Service.ILandpageService service;
        public LandpageController(Service.ILandpageService service)
        {
            this.service = service;
        }
        [HttpPost("landpage-message")]
        public IActionResult PostLandpageMessafe([FromBody] Object entities)
        {
            this.service.contact(entities);

            return Ok();
        }
    }
}
