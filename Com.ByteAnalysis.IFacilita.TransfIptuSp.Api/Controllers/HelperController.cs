using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        [HttpGet("document-type")]
        public IActionResult Get()
        {
            return Ok(new[] {
                new { code=1, description="Certidão de Matrícula do Registro de Imóvel" },
                new { code=2, description="Escritura de Compra e Venda (sem registro)" },
                new { code=3, description="Contrato de Compra e Venda" },
                new { code=4, description="Contrato de Cessão de Direitos sobre o Imóvel" },
                new { code=5, description="Formal de Partilha" },
                new { code=6, description="Sentença de Usucapião" },
                new { code=7, description="Outros" },
            });
        }
    }
}
