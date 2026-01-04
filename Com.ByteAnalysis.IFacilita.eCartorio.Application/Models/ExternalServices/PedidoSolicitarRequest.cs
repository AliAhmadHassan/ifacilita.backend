using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class PedidoSolicitarRequest
    {
        [JsonProperty("requerente")]
        public Requerente Requerente { get; set; }

        [JsonProperty("atos")]
        public IEnumerable<Ato> Atos { get; set; }

        [JsonProperty("informacaoComplementar")]
        public string InformacaoComplementar { get; set; }

        [JsonProperty("numeroProcesso")]
        public string NumeroProcesso { get; set; }

        [JsonProperty("origem")]
        public string Origem { get; set; }
    }

    public class Requerente
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

    public class Ato
    {
        [JsonProperty("ato")]
        public string AtoDescription { get; set; }

        [JsonProperty("municipio")]
        public string Municipio { get; set; }

        [JsonProperty("cartorios")]
        public IEnumerable<string> Cartorios { get; set; }

        public object DadosAto { get; set; }
    }
}
