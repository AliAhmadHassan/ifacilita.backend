using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuideRequestController : ControllerBase
    {
        private readonly IGuideRequestService _service;

        public GuideRequestController(IGuideRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post(GuideRequestInput guide)
        {
            var result = await _service.PostGuideRequestAsync(guide);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<GuideRequestInput>))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        [HttpGet("{page}/{count}/pending")]
        public async Task<IActionResult> GetPeding(int page, int count)
        {
            _ = page > 0 ? page-- : page;
            var result = await _service.GetGuideRequestPendingsAsync();
            return Ok(result.ToList().Skip(page * 10).Take(count));
        }


        [ProducesResponseType(200, Type = typeof(IEnumerable<GuideRequestInput>))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        [HttpGet("{page}/{count}/guidepending")]
        public async Task<IActionResult> GetGuidePeding(int page, int count)
        {
            _ = page > 0 ? page-- : page;
            var result = await _service.GetGuideRequestGuidePendingsAsync();
            if (result.Count() == 0)
                return NotFound(new { code = 404, message = "Guide peding not found" });

            return Ok(result.ToList().Skip(page * 10).Take(count));
        }

        [ProducesResponseType(200, Type = typeof(GuideRequestInput))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await _service.GetAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(IEnumerable<GuideRequestInput>))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAsync();
            return Ok(result);
        }

        [ProducesResponseType(200, Type = typeof(GuideRequestInput))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Put(GuideRequestInput guide)
        {
            var result = await _service.PostGuideRequestAsync(guide);
            return Ok(result);
        }
    }
}
