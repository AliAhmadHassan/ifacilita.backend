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
    public class BankController : ControllerBase
    {
        Service.IBankService service;

        public BankController(IBankService service)
        {
            this.service = service;
        }

        // GET: api/Bank
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/Bank/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/Bank
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Bank entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/Bank/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.Bank> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/Bank/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.Bank entity)
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
