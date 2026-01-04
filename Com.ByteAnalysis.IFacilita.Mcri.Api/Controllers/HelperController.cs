using Com.ByteAnalysis.IFacilita.Mcri.Service.Mapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Mcri.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private EnumMapperPropertyFractionType _enumMapperPropertyFractionType;
        private EnumMapperTypeOfContributor _enumMapperTypeOfContributor;
        private EnumMapperAcquisitionTitleType _enumMapperAcquisitionTitleType ;

        [HttpGet("PropertyFractionType")]
        public async Task<IActionResult> GetPropertyFractionType()
        {
            _enumMapperPropertyFractionType = new EnumMapperPropertyFractionType();
            return Ok(await Task.FromResult(_enumMapperPropertyFractionType.GetMapped()));
        }

        [HttpGet("TypeOfContributor")]
        public async Task<IActionResult> GetTypeOfContributor()
        {
            _enumMapperTypeOfContributor = new EnumMapperTypeOfContributor();
            return Ok(await Task.FromResult(_enumMapperTypeOfContributor.GetMapped()));
        }

        [HttpGet("AcquisitionTitleType")]
        public async Task<IActionResult> GetAcquisitionTitleType()
        {
            _enumMapperAcquisitionTitleType = new EnumMapperAcquisitionTitleType();
            return Ok(await Task.FromResult(_enumMapperAcquisitionTitleType.GetMapped()));
        }
    }
}
