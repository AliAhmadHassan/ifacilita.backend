using AutoMapper;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Errors;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [ProducesResponseType(200, Type = typeof(OrderInputDto))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _orderService.GetOrderAsync(id);
            return Ok(_mapper.Map<OrderInputDto>(result));
        }

        /// <summary>
        /// Gerar um pedido no eCartório do tipo Escrituras - Kit Escrituras
        /// </summary>
        /// <param name="orderInput"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(OrderInputDto))]
        [ProducesResponseType(404, Type = typeof(ProblemDetailsFields))]
        [ProducesResponseType(500, Type = typeof(ProblemDetailsFields))]
        [Route("ScriptureKit")]
        [HttpPost]
        public async Task<IActionResult> PostApplicant([FromBody] ApplicantInputDto orderInput)
        {
            var result = await _orderService.CreateByApplicantAsync(_mapper.Map<RequerenteInput>(orderInput));
            return Ok(_mapper.Map<OrderInputDto>(result));
        }
    }
}
