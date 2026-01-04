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
    public class BrokerController : ControllerBase
    {
        Service.IBrokerService service;

        public BrokerController(IBrokerService service)
        {
            this.service = service;
        }

        // GET: api/Broker
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/Broker/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/Broker
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Broker entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/Broker/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.Broker> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/Broker/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.Broker entity)
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
