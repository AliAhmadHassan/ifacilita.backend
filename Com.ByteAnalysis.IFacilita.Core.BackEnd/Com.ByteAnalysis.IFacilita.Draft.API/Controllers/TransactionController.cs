using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Com.ByteAnalysis.IFacilita.Draft.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Draft.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        Service.ITransactionService transactionService;

        public TransactionController(Service.ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public IActionResult get()
        {
            return Ok(this.transactionService.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(this.transactionService.Get(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Transaction transaction)
        {
            transaction = this.transactionService.Create(transaction);

            return Ok(new { id = transaction.Id});
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Transaction transaction, string id)
        {
            if (transactionService.Get(id) == null)
                return NotFound();

            this.transactionService.Update(id, transaction);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Transaction transaction)
        {
            if (transactionService.Get(transaction.Id) == null)
                return NotFound();

            this.transactionService.Remove(transaction);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] string id)
        {
            if (transactionService.Get(id) == null)
                return NotFound();

            this.transactionService.Remove(id);

            return Ok();
        }
    }
}
