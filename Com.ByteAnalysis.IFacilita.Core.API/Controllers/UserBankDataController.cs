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
    public class UserBankDataController : ControllerBase
    {
        Service.IUserBankDataService service;

        public UserBankDataController(IUserBankDataService service)
        {
            this.service = service;
        }

        // GET: api/UserBankData
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/UserBankData/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/UserBankData
        [HttpPost]
        public IActionResult Post([FromBody] Entity.UserBankData entity)
        {
            return Ok(this.service.Insert(entity));
        }

        // POST: api/UserBankData/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.UserBankData> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/UserBankData/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.UserBankData entity)
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
