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
    public class SendInviteTextTypeController : ControllerBase
    {
        Service.ISendInviteTextTypeService service;

        public SendInviteTextTypeController(ISendInviteTextTypeService service)
        {
            this.service = service;
        }

        // GET: api/SendInviteTextType
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/SendInviteTextType/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/SendInviteTextType
        [HttpPost]
        public IActionResult Post([FromBody] Entity.SendInviteTextType entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/SendInviteTextType/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.SendInviteTextType> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/SendInviteTextType/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.SendInviteTextType entity)
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
