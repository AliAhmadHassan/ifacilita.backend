using Newtonsoft.Json;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class ConsultarAtoResponse
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

        [JsonProperty("idItem")]
        public int IdItem { get; set; }

        public string UrlAct { get; set; }

        [JsonProperty("informacaoPedido")]
        public InformacaoPedido InformacaoPedido { get; set; }
    }

    public class InfoRequerente
    {
        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class InformacaoPedido
    {
        [JsonProperty("numeroPedido")]
        public long NumeroPedido { get; set; }

        [JsonProperty("dataPedido")]
        public DateTime DataPedido { get; set; }

        [JsonProperty("infoRequerente")]
        public InfoRequerente InfoRequerente { get; set; }
    }

}
