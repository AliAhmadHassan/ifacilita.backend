using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using Com.ByteAnalysis.IFacilita.TransfIptuSp.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferIptuController : ControllerBase
    {
        private readonly ITransferIptuService _service;

        public TransferIptuController(ITransferIptuService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _service.Get();
            if (result == null || result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {
            var result = _service.Get(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("pending")]
        public IActionResult GetPendings([FromQuery] int page, [FromQuery] int count)
        {
            try
            {
                _ = page > 0 ? page-- : page;
                var result = _service.GetPendings();

                if (result == null || result.Count() == 0)
                    return NotFound(new { message = "Transfer not found" });

                return Ok(result.ToList().Skip(page * 10).Take(count));

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequisitionModel entity)
        {
            try
            {
                entity.Created = System.DateTime.Now;
                entity.Updated = System.DateTime.Now;
                var result = _service.Create(entity);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] RequisitionModel requisition)
        {
            requisition.Updated = System.DateTime.Now;
            _service.Update(requisition.Id, requisition);
            return Ok(requisition);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            try
            {
                _service.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
