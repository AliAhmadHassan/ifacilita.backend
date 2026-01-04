using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionFlowController : ControllerBase
    {
        Service.ITransactionFlowService service;

        public TransactionFlowController(Service.ITransactionFlowService service)
        {
            this.service = service;
        }

        [HttpGet("{idtransaction}/idtransaction")]
        public IActionResult GetByIdtransaction(int idtransaction)
        {
            return Ok(this.service.FindByIdTransaction(idtransaction));
        }

    }
}
