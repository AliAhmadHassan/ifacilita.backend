using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class RecipientEvent
    {
        [JsonProperty("recipientEventStatusCode")]
        public string RecipientEventStatusCode { get; set; }

        [JsonProperty("includeDocuments")]
        public string IncludeDocuments { get; set; }
    }
}
