using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class EnvelopeEvent
    {
        [JsonProperty("envelopeEventStatusCode")]
        public string EnvelopeEventStatusCode { get; set; }

        [JsonProperty("includeDocuments")]
        public string IncludeDocuments { get; set; }
    }
}
