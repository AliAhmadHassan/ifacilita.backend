using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPushNotificationController : ControllerBase
    {
        Service.IPushNotificationService service;

        public UserPushNotificationController(Service.IPushNotificationService service)
        {
            this.service = service;
        }

        [HttpPost("{iduser}/{token}/add-token")]
        public IActionResult Post(int iduser, string token)
        {
            this.service.AddToken(iduser, token);

            return Ok();
        }
    }
}
