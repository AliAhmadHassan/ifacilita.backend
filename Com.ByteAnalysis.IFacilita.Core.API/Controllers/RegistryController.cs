using Com.ByteAnalysis.IFacilita.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        public readonly IRegistryService _service;

        public RegistryController(IRegistryService service)
        {
            this._service = service;
        }

        // GET: api/Registry
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._service.FindAll());
        }

        // GET: api/Registry/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this._service.FindById(id));
        }

        // GET: api/Registry/5
        [HttpGet("{idtransaction}/closer")]
        public IActionResult GetCloser(int idtransaction)
        {
            return Ok(this._service.FindCloser(idtransaction));
        }

        [HttpGet("city/{city}")]
        public IActionResult GetByCity([FromRoute] string city)
        {
            try
            {
                var result = _service.FindByCity(city);
                if (result == null || result.Count() == 0)
                    return NotFound(new { code = 404, message = "registry not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        // POST: api/Registry
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Registry entity)
        {
            return Ok(this._service.Insert(entity));
        }

        // POST: api/Registry/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.Registry> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this._service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/Registry/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.Registry entity)
        {
            return Ok(this._service.Update(entity));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this._service.Delete(id);

            return Ok();
        }
    }
}
