using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Service;
using Com.ByteAnalysis.IFacilita.Core.Service.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [EnableCors("CorsApi")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        Service.IUserService service;
        private readonly ITransactionFlowService _transactionFlowService;

        public UserController(IUserService service, ITransactionFlowService transactionFlowService)
        {
            this.service = service;
            _transactionFlowService = transactionFlowService;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.FindAll());
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(this.service.FindById(id));
        }


        [HttpGet("{authorizationCode}/social-login-authorization-code")]
        public IActionResult GetBySocialLoginAuthorizationCode(string authorizationCode)
        {
            Entity.User user = this.service.FindBySocialLoginAuthorizationCode(authorizationCode);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] Entity.User entity)
        {
            return Ok(this.service.Insert(entity));
        }
         
        // POST: api/User
        [HttpPost("with-social-login")]
        public IActionResult PostWithSocialLogin([FromBody] Entity.User entity)
        {
            return Ok(this.service.InsertWithSocialLogin(entity));
        }

        // POST: api/User
        [HttpPost("login")]
        public IActionResult PostForLogin([FromBody] Entity.User entity)
        {
            Entity.User user = null;

            try
            {
                user = this.service.Login(entity);
            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(user);
        }

        // POST: api/User/batch
        [HttpPost("batch ")]
        public IActionResult PostBatch([FromBody] List<Entity.User> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this.service.Insert(entity));
            }

            return Ok();
        }

        // PUT: api/User/5
        [HttpPost("miss-password")]
        public IActionResult MissPassword([FromBody] Entity.User entity)
        {
            try
            {
                this.service.MissPassword(entity);
            }
            catch (Exception error)
            {
                return  NotFound();
            }
            return Ok();
        }

        // PUT: api/User/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.User entity)
        {
            var result = this.service.Update(entity);
            return Ok(result);
        }

        [HttpPut("{idTransaction}/update-full")]
        public IActionResult PutFull([FromBody] Entity.User entity, [FromRoute] int idTransaction)
        {
            var result = this.service.Update(entity);
            

            switch (entity.IdUserProfile)
            {
                case 1: //comprador
                    _ = UpdateTransactionFlow(idTransaction, 3, 2);
                    break;
                case 2: //vendedor
                    _ = UpdateTransactionFlow(idTransaction, 1, 2);
                    break;
                default: //Corretor
                    break;
            }

            return Ok(result);
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }

        private bool UpdateTransactionFlow(int idTransaction, int idFlow, int status)
        {
            IEnumerable<TransactionFlow> transactionFlows = _transactionFlowService.FindByIdTransaction(idTransaction);
            TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(idFlow)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = status;
                transactionFlow.StatusChanged = DateTime.Now;
                _transactionFlowService.Update(transactionFlow);
            }

            return true;
        }
    }
}
