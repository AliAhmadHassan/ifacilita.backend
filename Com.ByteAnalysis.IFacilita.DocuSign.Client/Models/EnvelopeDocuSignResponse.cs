using Newtonsoft.Json;
using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class EnvelopeDocuSignResponse
    {
        [JsonProperty("envelopeId")]
        public string EnvelopeId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDateTime")]
        public DateTimeOffset StatusDateTime { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
