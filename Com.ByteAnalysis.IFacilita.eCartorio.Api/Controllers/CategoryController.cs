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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            var result = await _categoryService.List(city);
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(result));
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<CertificateByCategoryDto>))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [Route("Certificates")]
        [HttpGet]
        public async Task<IActionResult> GetCertificates([FromQuery] int idCategory, [FromQuery] string city)
        {
            var result = await _categoryService.CertificatesByCategory(idCategory, city);
            return Ok(_mapper.Map<IEnumerable<CertificateByCategoryDto>>(result));
        }
    }
}
