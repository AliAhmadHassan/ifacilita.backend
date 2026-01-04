using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.EmailService;
using Com.ByteAnalysis.IFacilita.Common.Impl;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Entity.Dto.Patrimony;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using Com.ByteAnalysis.IFacilita.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatrimonyController : ControllerBase
    {
        private readonly IPatrimonyService _patrimonyService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionFlowRepository _transactionFlowRepository;
        private readonly IConfiguration _configuration;

        private IS3 _s3;

        public PatrimonyController(
            IPatrimonyService service,
            ITransactionService transactionService,
            IConfiguration configuration,
            ITransactionFlowRepository transactionFlowRepository)
        {
            this._patrimonyService = service;
            _transactionService = transactionService;
            _transactionFlowRepository = transactionFlowRepository;
            _configuration = configuration;

            _s3 = new S3();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._patrimonyService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = this._patrimonyService.FindById(id);
            return Ok(result);
        }

        [HttpGet("{id}/itbi-robot")]
        public IActionResult GetItbiRobot(string id)
        {
            return Ok(this._patrimonyService.FindItbiRobot(id));
        }

        [HttpGet("{id}/itbi-robot-sp")]
        public IActionResult GetItbiSpRobot(string id)
        {
            return Ok(_patrimonyService.FindItbiSpRobot(id));
        }

        [HttpGet("{transactionId}/callback-itbi-sp")]
        public IActionResult CallbackItbiSp(int transactionId)
        {
            try
            {
                var transaction = _transactionService.FindById(transactionId);
                var itbi = _patrimonyService.FindItbiSpRobot(transaction.Patrimony.IdItbiRobot);
                dynamic itbiDynamic = JValue.Parse(itbi.ToString());
                var value = itbiDynamic.urlBillet.Value;

                if (value != null)
                {
                    List<Tuple<string, string>> tos = new List<Tuple<string, string>>();

                    tos.Add(new Tuple<string, string>(transaction.User_Seller.Name, transaction.User_Seller.EMail));
                    tos.Add(new Tuple<string, string>(transaction.User.Name, transaction.User.EMail));

                    var resultSendBillet = new ClientEmailService(_configuration)
                        .SendMail(tos, null, null, null, new List<object>() { new { key = "UrlBillet", value } }, "BilletItbi");

                }

                return Ok(new { code = 200, message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{transactionId}/reset-itbi")]
        public IActionResult ItbiReset(int transactionId)
        {
            try
            {
                var transaction = _transactionService.FindById(transactionId);

                transaction.Patrimony.IdItbiRobot = null;
                _patrimonyService.Update(transaction.Patrimony);

                IEnumerable<TransactionFlow> transactionFlows = _transactionFlowRepository.findByIdTransaction(transactionId);
                List<int> listFlowItbi = new List<int>() { 1036, 1037, 1038, 1039, 1040, 1041, 1042 };
                List<TransactionFlow> transactionSubFlows = transactionFlows.Where(c => listFlowItbi.Contains(c.IdplatformSubWorkflow)).ToList();

                foreach (var transactionSubFlow in transactionSubFlows)
                {
                    transactionSubFlow.Status = 0;
                    transactionSubFlow.StatusChanged = DateTime.Now;
                    _transactionFlowRepository.Update(transactionSubFlow);
                }

                return Ok(new { code = 200, message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Entity.Patrimony entity)
        {
            return Ok(this._patrimonyService.Insert(entity));
        }

        [HttpPost("{idTransaction}/upload-voucher"), DisableRequestSizeLimit]
        public IActionResult UploadVoucher([FromRoute] int idTransaction)
        {
            try
            {
                if (Request.Form.Files.ToArray().Length == 0)
                    return BadRequest("Nenhum arquivo encontrado para upload");

                var file = Request.Form.Files[0];

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "vouchers");
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                var transaction = _transactionService.FindById(idTransaction);
                if (transaction == null)
                    return BadRequest("Transação não encontrada");


                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(fullPath));
                    transaction.ItbiVoucher = base64;
                    transaction.ItbiVoucher_FileName = fileName;

                    _transactionService.Update(transaction);

                    return Ok(new { code = 200, message = "ok" });
                }
                else
                {
                    return BadRequest(new { code = 400, message = "Não foi possível fazer o upload do arquivo" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("upload-declaration")]
        public IActionResult UploadDeclaration([FromBody] UploadDeclarationDto declarationDto)
        {
            try
            {
                var pathS3 = _s3.SaveFile(declarationDto.DeclarationBase64, declarationDto.FileName.Split('.')[1]);
                var urlDocument = Path.Combine("https://ifacilita.s3.us-east-2.amazonaws.com", pathS3);

                var patrimony = _patrimonyService.FindById(declarationDto.IdPatrimony);
                patrimony.CondominiumDeclarationDebts = urlDocument;

                _ = _patrimonyService.Update(patrimony);

                return Ok(new { code = 200, message = "ok", urlDocument });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, ex.Message,  });
            }
        }

        [HttpGet("{idTransaction}/confirm-itbi-data-buyer")]
        public IActionResult ConfirmItbiDataByBuyer([FromRoute] int idTransaction)
        {
            try
            {
                IEnumerable<TransactionFlow> transactionFlows = _transactionFlowRepository.findByIdTransaction(idTransaction);
                TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(1039)).FirstOrDefault();
                if (transactionFlow != null)
                {
                    transactionFlow.Status = 2;
                    transactionFlow.StatusChanged = DateTime.Now;
                    _transactionFlowRepository.Update(transactionFlow);
                }

                return Ok(new { code = 200, message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        [HttpPost("itbi-robot")]
        public IActionResult PostItbiRobot([FromBody] Entity.Transaction entity)
        {
            this._patrimonyService.ItbiRobot(entity);
            return Ok();
        }

        [HttpPost("itbi-robot-approved")]
        public IActionResult PostItbiRobotApproved([FromBody] Entity.Transaction entity)
        {
            this._patrimonyService.ItbiRobotApproved(entity);
            return Ok();
        }

        [HttpPost("batch")]
        public IActionResult PostBatch([FromBody] List<Entity.Patrimony> entities)
        {
            foreach (var entity in entities)
            {
                return Ok(this._patrimonyService.Insert(entity));
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Entity.Patrimony entity)
        {
            return Ok(this._patrimonyService.Update(entity));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this._patrimonyService.Delete(id);

            return Ok();
        }
    }
}
