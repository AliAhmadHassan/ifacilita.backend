using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricController : ControllerBase
    {
        Service.IHistoricService service;

        public HistoricController(Service.IHistoricService service)
        {
            this.service = service;
        }

        // GET: api/Historic
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/Historic/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/Historic/5
        [HttpGet("{id}/idtransaction")]
        public IActionResult GetByIdTransaction(int id)
        {
            return Ok(this.service.FindByIdTransaction(id));
        }

        // POST: api/Historic
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Historic entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // PUT: api/Historic
        [HttpPut]
        public IActionResult Put([FromBody] Entity.Historic entity)
        {
            return Ok(this.service.Update(entity));
        }

        // DELETE: api/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }
    }
}
