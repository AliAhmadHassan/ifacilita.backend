using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;

        public KitController(IKitService kitService, IMapper mapper)
        {
            _kitService = kitService;
            _mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<KitDto>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string municipio)
        {
            var result = await _kitService.ListAsync(municipio);

            return Ok( _mapper.Map<IEnumerable<KitDto>>(result));
        }

        [Route("CertiticatesByKit")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<KitCertidoesPorKitResponse>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> CertiticatesByKit([FromQuery] int idKit, [FromQuery] string municipio)
        {
            var result = await _kitService.GetCertificatesByKitAsync(idKit, municipio);
            return Ok( _mapper.Map<IEnumerable< CertificateByKitDto>>(result));
        }

    }
}
