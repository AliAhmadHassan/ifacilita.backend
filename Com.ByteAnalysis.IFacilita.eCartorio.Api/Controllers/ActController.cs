using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActController : ControllerBase
    {
        private readonly IActService _actService;
        private readonly IMapper _mapper;

        public ActController(IActService actService, IMapper mapper)
        {
            _actService = actService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma ato a patir do código CERP
        /// </summary>
        /// <param name="cerp">Identificador CERP da certidão eletrônica</param>
        /// <response code="200">Tudo Certo</response>
        /// <response code="400">Não encontrado</response>
        /// <response code="500">Erro interno</response>
        [ProducesResponseType(200, Type = typeof(ActDto))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string cerp)
        {
            var result = await _actService.GetAsync(cerp);
            return Ok(_mapper.Map<ActDto>(result));
        }

        [ProducesResponseType(200, Type = typeof(object[]))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("registry")]
        public async Task<IActionResult> GetByAct([FromQuery] int actId,[FromQuery]string city)
        {
            var result = await _actService.GetRegistryByActAsync(actId, city);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [Route("download")]
        [HttpGet]
        public async Task<IActionResult> Download([FromQuery] string cerp)
        {
            var result = await _actService.DownloadAsync(cerp);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(object))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [Route("view")]
        [HttpPost]
        public async Task<IActionResult> View([FromQuery] string cerp)
        {
            var result = await _actService.ViewAsync(cerp);
            return Ok(result);
        }
    }
}
