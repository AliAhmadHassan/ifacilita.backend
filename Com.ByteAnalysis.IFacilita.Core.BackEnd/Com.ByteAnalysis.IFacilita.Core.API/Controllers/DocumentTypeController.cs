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
    public class DocumentTypeController : ControllerBase
    {
        Service.IDocumentTypeService service;

        public DocumentTypeController(IDocumentTypeService service)
        {
            this.service = service;
        }

        // GET: api/DocumentType
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/DocumentType/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/DocumentType
        [HttpPost]
        public IActionResult Post([FromBody] Entity.DocumentType entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/DocumentType/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.DocumentType> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/DocumentType/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.DocumentType entity)
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
