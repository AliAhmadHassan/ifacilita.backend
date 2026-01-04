using AutoMapper;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Errors;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocuSignController : ControllerBase
    {
        private readonly IUserDocuSignService _service;
        protected readonly IMapper _mapper;

        public UserDocuSignController(IUserDocuSignService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAsync();
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}/api")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(UserGetOutput))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}/docusign")]
        public async Task<IActionResult> GetDocuSign(string id)
        {
            var result = await _service.GetDocuSignAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInput input)
        {
            var res = await _service.PostDocuSign(input);

            return Ok(new User() { Email = res.NewUsers[0].Email, UserIdDocuSign = res.NewUsers[0].UserId, UserName = res.NewUsers[0].UserName });
        }

        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.RemoveAsync(id);
            return Ok();
        }

    }
}
