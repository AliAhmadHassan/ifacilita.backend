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
    public class TransactionController : ControllerBase
    {
        Service.ITransactionService service;

        public TransactionController(ITransactionService service)
        {
            this.service = service;
        }

        // GET: api/Transaction
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // GET: api/Transaction/5
        [HttpGet("{id}/promise-document")]
        public IActionResult GetPromiseDocument(int id)
        {
            return Ok(new { doc = this.service.MakePromiseDocument(id).Replace("\\r", "") });
        }

        // GET: api/Transaction/5
        [HttpGet("{id}/call-docusign")]
        public IActionResult GetCallDocusign(int id)
        {
            return Ok(new { doc = this.service.CallDocusign(id) });
        }

        [HttpGet("{id}/call-docusign-ver-final")]
        public IActionResult GetCallDocusignVFinal(int id)
        {
            return Ok(new { doc = this.service.CallDocusignVFinal(id) });
        }

        [HttpGet("callback-docusign")]
        public IActionResult GetCallbackDocusign()
        {
            try
            {
                System.IO.File.AppendAllText("callbackdocusign.log", Newtonsoft.Json.JsonConvert.SerializeObject(System.Net.WebUtility.UrlDecode(Request.QueryString.ToString())));
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText("callbackdocusign.log", Request.QueryString.ToString());
            }
            return Ok();
        }

        // POST: api/Transaction
        [HttpPost]
        public IActionResult Post([FromBody] Entity.Transaction entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/Transaction/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.Transaction> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        [HttpPost("recive-contract")]
        public IActionResult PostReciveContract([FromBody] Service.ViewModel.ContractViewModel entity)
        {
            
            return Ok(new { ContractToken = this.service.ReciveContract(entity) });
        }

        // PUT: api/Transaction/5
        [HttpPut("agreed-signal")]
        public IActionResult PutAgreeSignal([FromBody] Entity.Transaction entity)
        {
            this.service.BuyerAgreeSignal(entity);
            return Ok();
        }

        [HttpPut("recived-signal-value")]
        public IActionResult PutRecivedSignalValue([FromBody] Entity.Transaction entity)
        {
            this.service.BuyerRecivedSignalValue(entity);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity.Transaction entity)
        {
            return Ok(this.service.Update(entity));
        }


        [HttpPut("inform-key-condition")]
        public IActionResult InformKeyCondition([FromBody] Entity.Transaction entity)
        {
            return Ok(this.service.InformKeyCondition(entity));
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
