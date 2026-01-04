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
    public class RegistryController : ControllerBase
    {
        Service.IRegistryService service;

        public RegistryController(IRegistryService service)
        {
            this.service = service;
        }

        // GET: api/Registry
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/Registry/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/Registry
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Registry entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/Registry/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.Registry> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/Registry/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.Registry entity)
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
