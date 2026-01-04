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
    public class TransactionPaymentFormController : ControllerBase
    {
        Service.ITransactionPaymentFormService service;

        public TransactionPaymentFormController(ITransactionPaymentFormService service)
        {
            this.service = service;
        }

        // GET: api/TransactionPaymentForm
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/TransactionPaymentForm/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/TransactionPaymentForm/5
        [HttpGet("{idtransaction}/idtransaction")]
        public IActionResult GetByIdtransaction(int idtransaction)
        {
            return Ok(this.service.FindByIdtransaction(idtransaction));
        }

        // POST: api/TransactionPaymentForm
        [HttpPost]
        public IActionResult Post([FromBody] Entity.TransactionPaymentForm entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/TransactionPaymentForm/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.TransactionPaymentForm> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/TransactionPaymentForm/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.TransactionPaymentForm entity)
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
