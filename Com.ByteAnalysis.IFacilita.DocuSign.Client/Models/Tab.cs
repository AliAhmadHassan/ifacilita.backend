using Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class Tab
    {
        [JsonProperty("anchorString")]
        public string AnchorString { get; set; }

        [JsonProperty("anchorYOffset")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long AnchorYOffset { get; set; }

        [JsonProperty("fontSize")]
        public string FontSize { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("recipientId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long RecipientId { get; set; }

        [JsonProperty("tabLabel")]
        public string TabLabel { get; set; }
    }
}
