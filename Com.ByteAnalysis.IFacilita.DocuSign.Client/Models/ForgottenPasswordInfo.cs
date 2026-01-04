using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class ForgottenPasswordInfo
    {
        [JsonProperty("forgottenPasswordQuestion1")]
        public string ForgottenPasswordQuestion1 { get; set; }

        [JsonProperty("forgottenPasswordAnswer1")]
        public string ForgottenPasswordAnswer1 { get; set; }

        [JsonProperty("forgottenPasswordQuestion2")]
        public string ForgottenPasswordQuestion2 { get; set; }

        [JsonProperty("forgottenPasswordAnswer2")]
        public string ForgottenPasswordAnswer2 { get; set; }

        [JsonProperty("forgottenPasswordQuestion3")]
        public string ForgottenPasswordQuestion3 { get; set; }

        [JsonProperty("forgottenPasswordAnswer3")]
        public string ForgottenPasswordAnswer3 { get; set; }

        [JsonProperty("forgottenPasswordQuestion4")]
        public string ForgottenPasswordQuestion4 { get; set; }

        [JsonProperty("forgottenPasswordAnswer4")]
        public string ForgottenPasswordAnswer4 { get; set; }
    }
}
