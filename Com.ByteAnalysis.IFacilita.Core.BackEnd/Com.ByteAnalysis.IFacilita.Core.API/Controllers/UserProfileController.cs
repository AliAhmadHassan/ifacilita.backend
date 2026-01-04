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
    public class UserProfileController : ControllerBase
    {
        Service.IUserProfileService service;

        public UserProfileController(IUserProfileService service)
        {
            this.service = service;
        }

        // GET: api/UserProfile
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/UserProfile/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/UserProfile
        [HttpPost]
        public IActionResult Post([FromBody] Entity.UserProfile entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/UserProfile/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.UserProfile> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/UserProfile/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.UserProfile entity)
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
