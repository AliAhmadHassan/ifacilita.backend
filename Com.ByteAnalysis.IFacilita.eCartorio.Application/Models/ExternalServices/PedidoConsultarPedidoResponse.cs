using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class PedidoConsultarPedidoResponse
    {
        [JsonProperty("numeroPedido")]
        public long NumeroPedido { get; set; }

        [JsonProperty("dataPedido")]
        public DateTime DataPedido { get; set; }

        [JsonProperty("quantidadeAtos")]
        public int QuantidadeAtos { get; set; }

        [JsonProperty("valorPedido")]
        public decimal ValorPedido { get; set; }

        [JsonProperty("requerente")]
        public Requerente Requerente { get; set; }

        [JsonProperty("atos")]
        public IEnumerable<AtoConsultaPedidoResponse> Atos { get; set; }
    }

    public class AtoConsultaPedidoResponse
    {
        [JsonProperty("numeroAto")]
        public long NumeroAto { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("certidao")]
        public string Certidao { get; set; }

        [JsonProperty("cartorio")]
        public string Cartorio { get; set; }

        [JsonProperty("finalidade")]
        public string Finalidade { get; set; }

        [JsonProperty("dataPagamento")]
        public DateTime? DataPagamento { get; set; }

        [JsonProperty("cerp")]
        public string Cerp { get; set; }

        [JsonProperty("tipoAto")]
        public string TipoAto { get; set; }
    }
}
