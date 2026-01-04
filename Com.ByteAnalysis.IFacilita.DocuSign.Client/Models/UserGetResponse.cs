using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class UserGetResponse
    {
        [JsonProperty("users")]
        public UserResponse[] Users { get; set; }

        [JsonProperty("resultSetSize")]
        public string ResultSetSize { get; set; }

        [JsonProperty("totalSetSize")]
        public string TotalSetSize { get; set; }

        [JsonProperty("startPosition")]
        public string StartPosition { get; set; }

        [JsonProperty("endPosition")]
        public string EndPosition { get; set; }

        [JsonProperty("nextUri")]
        public string NextUri { get; set; }

        [JsonProperty("previousUri")]
        public string PreviousUri { get; set; }
    }
}
