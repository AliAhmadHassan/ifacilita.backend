using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class PedidoSolicitarResponse
    {
        [JsonProperty("numeroPedido")]
        public long NumeroPedido { get; set; }

        [JsonProperty("quantidadeCertidoesSolicitadas")]
        public int QuantidadeCertidoesSolicitadas { get; set; }

        [JsonProperty("valorPedido")]
        public decimal ValorPedido { get; set; }
    }

    public class OrderResponseDto
    {
        public long OrderNumber { get; set; }

        public int QuantityCertificates { get; set; }

        public decimal OrderValue { get; set; }
    }
}
