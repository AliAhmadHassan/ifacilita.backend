using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class ExigenciaListarResponse
    {
        [JsonProperty("idExigencia")]
        public int IdExigencia { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dataRegistro")]
        public DateTime? DataRegistro { get; set; }

        [JsonProperty("dataPrazo")]
        public DateTime? DataPrazo { get; set; }

        [JsonProperty("dataCumprimento")]
        public DateTime? DataCumprimento { get; set; }

        [JsonProperty("valorExigencia")]
        public decimal? ValorExigencia { get; set; }

        [JsonProperty("dataLiberacao")]
        public DateTime? DataLiberacao { get; set; }

        [JsonProperty("tipoExigencia")]
        public string TipoExigencia { get; set; }

        [JsonProperty("mensagens")]
        public List<Mensagem> Mensagens { get; set; }
    }

    public class Mensagem
    {
        [JsonProperty("remetente")]
        public string Remetente { get; set; }

        [JsonProperty("mensagem")]
        public string MensagemText { get; set; }

        [JsonProperty("dataHora")]
        public DateTime DataHora { get; set; }
    }
}
