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
    public class UserController : ControllerBase
    {
        Service.IUserService service;

        public UserController(Service.IUserService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(this.service.Get(id));
        }

        [HttpPost]
        public IActionResult Post(Model.User user)
        {
            user = this.service.CreateOrUpdate(user);
            return Ok(user.Id);
        }

        [HttpPut]
        public IActionResult Put(Model.User user)
        {
            user = this.service.CreateOrUpdate(user);
            return Ok(user.Id);
        }

        [HttpPut("{id}/{token}/add-token")]
        public IActionResult AddToken(string id, string token)
        {
            return Ok(this.service.AddToken(id, token).Id);
        }
    }
}
