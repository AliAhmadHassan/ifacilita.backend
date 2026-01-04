using Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public partial class EnvelopeDocuSign
    {
        [JsonProperty("documents")]
        public Document[] Documents { get; set; }

        [JsonProperty("emailSubject")]
        public string EmailSubject { get; set; }

        [JsonProperty("recipients")]
        public Recipients Recipients { get; set; }

        [JsonProperty("eventNotification")]
        public EventNotification EventNotification { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class EnvelopeDocuSign
    {
        public static EnvelopeDocuSign FromJson(string json) => JsonConvert.DeserializeObject<EnvelopeDocuSign>(json, Converter.Settings);
    }
}
