using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway;
using Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IClientApiPaymentsGateway _paymentsGateway;
        public PaymentController(IClientApiPaymentsGateway paymentsGateway)
        {
            _paymentsGateway = paymentsGateway;
        }

        [HttpPost("client")]
        public async Task<IActionResult> PostClient(ClientDto client)
        {
            try
            {
                var result = await _paymentsGateway.CreateClientAsync(client);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }

        [HttpPost("invoice")]
        public async Task<IActionResult> PostInvoice(InvoiceDto invoice)
        {
            try
            {
                var result = await _paymentsGateway.CreateInvoiceAsync(invoice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, message = ex.Message });
            }
        }


    }
}
