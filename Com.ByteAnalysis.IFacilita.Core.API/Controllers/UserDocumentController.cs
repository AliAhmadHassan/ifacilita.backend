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
    public class UserDocumentController : ControllerBase
    {
        Service.IUserDocumentService service;

        public UserDocumentController(IUserDocumentService service)
        {
            this.service = service;
        }

        // GET: api/UserDocument
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/UserDocument/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/UserDocument/5
        [HttpGet("{iduser}/iduser")]
        public IActionResult GetByIduser(int iduser)
        {
            return Ok(this.service.FindByIdUser(iduser));
        }

        // POST: api/UserDocument
        [HttpPost]
        public IActionResult Post([FromBody] Entity.UserDocument entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/UserDocument/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.UserDocument> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/UserDocument/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.UserDocument entity)
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
