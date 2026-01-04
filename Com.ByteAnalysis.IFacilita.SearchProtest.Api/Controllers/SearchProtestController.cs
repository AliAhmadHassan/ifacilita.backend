using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using Com.ByteAnalysis.IFacilita.SearchProtest.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchProtestController : ControllerBase
    {
        private readonly ISearchProtestService _service;

        public SearchProtestController(ISearchProtestService service)
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
                    return NotFound(new { message = "Cri not found" });

                return Ok(result.ToList().Skip(page * 10).Take(count));

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestSearchProtestModel entity)
        {
            try
            {
                entity.StatusDownloadCertificates = "Unprocessed";
                var result = _service.Create(entity);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] RequestSearchProtestModel requisition)
        {
            
            _service.Update(requisition.Id, requisition);

            if (!string.IsNullOrEmpty(requisition.UrlCallback))
            {
                List<GlobalError> errors = new List<GlobalError>();
                if (requisition.Errors != null)
                    errors = requisition.Errors.ToList();

                try
                {
                    using var clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    using var client = new HttpClient(clientHandler);
                    var content = new StringContent(JsonConvert.SerializeObject(new { requisition.Id, orderId = "", certiticateType = "PropertyRegistrationData" }), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(requisition.UrlCallback, content).Result;

                    requisition.UrlCallbackResponse = $"O servidor {requisition.UrlCallback} de callback retornou o status: {response.StatusCode}, mensagem: {response.Content.ReadAsStringAsync().Result}";
                }
                catch (Exception ex)
                {
                    errors.Add(new GlobalError()
                    {
                        Message = $"Houve uma falha ao tentar atualizar os dados da requisição. " + ex.Message,
                        Code = 0,
                        Field = "Put"
                    });
                }

                requisition.Errors = errors;
                _service.Update(requisition.Id, requisition);
            }

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
