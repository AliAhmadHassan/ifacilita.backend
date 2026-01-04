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
    public class PatrimonyIptuController : ControllerBase
    {
        Service.IPatrimonyIptuService service;

        public PatrimonyIptuController(IPatrimonyIptuService service)
        {
            this.service = service;
        }

        // GET: api/PatrimonyIptu
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/PatrimonyIptu/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/PatrimonyIptu/5
        [HttpGet("{id}/patrimony_municipal_registration")]
        public IActionResult Get(string patrimony_municipal_registration)
        {
            return Ok(this.service.FindByPatrimonyMunicipalRegistration(patrimony_municipal_registration));
        }


        // POST: api/PatrimonyIptu
        [HttpPost]
        public IActionResult Post([FromBody] Entity.PatrimonyIptu entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/PatrimonyIptu/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.PatrimonyIptu> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/PatrimonyIptu/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.PatrimonyIptu entity)
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
