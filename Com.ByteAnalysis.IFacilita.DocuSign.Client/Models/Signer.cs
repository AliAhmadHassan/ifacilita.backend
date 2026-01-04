using Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class Signer
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("recipientId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long RecipientId { get; set; }

        [JsonProperty("routingOrder")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long RoutingOrder { get; set; }

        [JsonProperty("tabs")]
        public Tabs Tabs { get; set; }
    }
}
