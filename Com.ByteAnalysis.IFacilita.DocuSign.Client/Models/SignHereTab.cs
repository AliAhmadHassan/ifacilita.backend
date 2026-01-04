using Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class SignHereTab
    {
        [JsonProperty("anchorString")]
        public string AnchorString { get; set; }

        [JsonProperty("anchorUnits")]
        public string AnchorUnits { get; set; }

        [JsonProperty("anchorXOffset")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long AnchorXOffset { get; set; }

        [JsonProperty("anchorYOffset")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long AnchorYOffset { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("optional")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool Optional { get; set; }

        [JsonProperty("recipientId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long RecipientId { get; set; }

        [JsonProperty("scaleValue")]
        public long ScaleValue { get; set; }

        [JsonProperty("tabLabel")]
        public string TabLabel { get; set; }
    }
}
