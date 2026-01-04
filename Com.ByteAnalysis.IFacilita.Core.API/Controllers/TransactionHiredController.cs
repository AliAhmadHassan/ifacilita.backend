using Com.ByteAnalysis.IFacilita.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHiredController : ControllerBase
    {
        Service.ITransactionHiredService service;

        public TransactionHiredController(ITransactionHiredService service)
        {
            this.service = service;
        }

        // GET: api/City
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/City/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/City/5
        [HttpGet("{idTransaction}/idtransaction")]
        public IActionResult GetByIdTransaction(int idTransaction)
        {
            return Ok(this.service.FindByIdTransaction(idTransaction));
        }

        // POST: api/City
        [HttpPost]
        public IActionResult Post([FromBody] Entity.TransactionHired entity)
        {
            return Ok(this.service.Insert(entity));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity.TransactionHired entity)
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
