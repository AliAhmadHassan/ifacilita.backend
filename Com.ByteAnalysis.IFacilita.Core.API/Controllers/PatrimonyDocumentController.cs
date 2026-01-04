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
    public class PatrimonyDocumentController : ControllerBase
    {
        Service.IPatrimonyDocumentService service;

        public PatrimonyDocumentController(IPatrimonyDocumentService service)
        {
            this.service = service;
        }

        // GET: api/PatrimonyDocument
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/PatrimonyDocument/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/PatrimonyDocument
        [HttpPost]
        public IActionResult Post([FromBody] Entity.PatrimonyDocument entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/PatrimonyDocument/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.PatrimonyDocument> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/PatrimonyDocument/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.PatrimonyDocument entity)
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
