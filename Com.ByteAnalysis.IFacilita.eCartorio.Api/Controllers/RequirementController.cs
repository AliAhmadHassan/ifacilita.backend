using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly IRequirementService _requirementService;
        private readonly IMapper _mapper;

        public RequirementController(IRequirementService requirementService, IMapper mapper)
        {
            _requirementService = requirementService;
            _mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<RequirementDto>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> Get(string cerp)
        {
            var result = await _requirementService.GetAsync(cerp);
            return Ok(_mapper.Map<IEnumerable<RequirementDto>>(result));
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<RequirementResponse>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [Route("applicant")]
        [HttpGet]
        public async Task<IActionResult> GetByApplicant(string document)
        {
            var result = await _requirementService.GetByApplicantAsync(document);
            return Ok(_mapper.Map<IEnumerable<RequirementDto>>(result));
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int idRequirement, string message)
        {
            await _requirementService.PostAsync(idRequirement, message);
            return Ok();
        }
    }
}
