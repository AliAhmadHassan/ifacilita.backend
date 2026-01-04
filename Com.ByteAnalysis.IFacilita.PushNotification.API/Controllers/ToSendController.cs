using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.PushNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToSendController : ControllerBase
    {
        Service.IToSendService service;

        public ToSendController(Service.IToSendService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(this.service.Get(id));
        }

        [HttpPost("{iduser}")]
        public IActionResult Post(string iduser, Model.Notification notification)
        {
            this.service.CreateOrUpdate(iduser, notification);
            return Ok();
        }

    }
}
