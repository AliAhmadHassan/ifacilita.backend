using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.ByteAnalysis.IFacilita.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        Service.IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }


        [HttpGet("{authorizationCode}/social-login-authorization-code")]
        public IActionResult GetBySocialLoginAuthorizationCode(string authorizationCode)
        {
            Entity.User user = this.service.FindBySocialLoginAuthorizationCode(authorizationCode);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] Entity.User entity)
        {
            return Ok(this.service.Insert(entity));
        }
         
        // POST: api/User
        [HttpPost("with-social-login")]
        public IActionResult PostWithSocialLogin([FromBody] Entity.User entity)
        {
            return Ok(this.service.InsertWithSocialLogin(entity));
        }

        // POST: api/User/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.User> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/User/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.User entity)
        {
            return Ok(this.service.Update(entity));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }
    }
}
