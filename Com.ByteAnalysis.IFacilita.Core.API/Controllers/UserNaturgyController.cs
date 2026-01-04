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
    public class UserNaturgyController : ControllerBase
    {

        Service.IUserNaturgyService service;

        public UserNaturgyController(IUserNaturgyService service)
        {
            this.service = service;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }

        [HttpGet("clientId/{client_id}")]
        public IActionResult GetIdUser(int idUser)
        {
            return Ok(this.service.findByIdUser(idUser));
        }

        [HttpGet("treatedRobot/{treatedRobot}")]
        public IActionResult Get(bool treatedRobot)
        {
            return Ok(this.service.FindByTreatedRobot(treatedRobot));
        }

        // POST: api/Address
        [HttpPost]
        public IActionResult Post([FromBody] Entity.UserNaturgy entity)
        {
            return Ok(this.service.Insert(entity));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity.UserNaturgy entity)
        {
            return Ok(this.service.Update(entity));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }
    }



}
