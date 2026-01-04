using Com.ByteAnalysis.IFacilita.DocuSign.Application.Errors;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvelopeDocuSignController : ControllerBase
    {
        private readonly IEnvelopeDocuSignService _service;

        public EnvelopeDocuSignController(IEnvelopeDocuSignService service)
        {
            _service = service;
        }

        [ProducesResponseType(200, Type = typeof(EnvelopeOutput))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnvelopeInput envelope)
        {
            var result = await _service.PostDocuSign(envelope);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(EnvelopeOutput))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}/api")]
        public async Task<IActionResult> GetInApi(string id)
        {
            var result = await _service.GetAsync(id);
            if (result == null)
                return NotFound(new { code = 404, message = "Envelope not found" });

            List<DocumentReponse> documents = new List<DocumentReponse>();

            result.DocumentsResponse.ForEach(x =>
            {
                documents.Add(new DocumentReponse()
                {
                    DateReceived = x.DateReceived,
                    Status = x.Status,
                    Url = x.Url
                });
            });

            return Ok(new EnvelopeOutput()
            {
                EnvelopeId = result.EnvelopeId,
                Status = result.Status,
                StatusDateTime = result.StatusDateTime,
                Uri = result.Uri,
                DocumentsCallback = documents
            });
        }

        [ProducesResponseType(200, Type = typeof(EnvelopeOutput))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}/envelope/api")]
        public async Task<IActionResult> GetInApiByEnvelopeId(string id)
        {
            var result = await _service.GetByEnvelopeIdAsync(id);
            if (result == null)
                return NotFound(new { code = 404, message = "Envelope not found" });

            List<DocumentReponse> documents = new List<DocumentReponse>();

            result.DocumentsResponse?.ForEach(x =>
            {
                documents.Add(new DocumentReponse()
                {
                    DateReceived = x.DateReceived,
                    Status = x.Status,
                    Url = x.Url,
                    FileName = x.FileName
                });
            });

            return Ok(new EnvelopeOutput()
            {
                EnvelopeId = result.EnvelopeId,
                Status = result.Status,
                StatusDateTime = result.StatusDateTime,
                Uri = result.Uri,
                DocumentsCallback = documents
            });
        }

        [ProducesResponseType(200, Type = typeof(EnvelopeGetOutput))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}/docusign")]
        public async Task<IActionResult> GetInDocuSign(string id)
        {
            var result = await _service.GetDocuSignAsync(id);
            return Ok(result);
        }
    }
}
