using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class UserDocuSignResponse
    {
        [JsonProperty("newUsers")]
        public UserResponse[] NewUsers { get; set; }
    }
}
