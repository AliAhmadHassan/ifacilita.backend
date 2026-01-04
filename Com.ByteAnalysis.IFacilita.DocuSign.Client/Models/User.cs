using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class User
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userType")]
        public string UserType { get; set; }

        [JsonProperty("userStatus")]
        public string UserStatus { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("loginStatus")]
        public string LoginStatus { get; set; }

        [JsonProperty("sendActivationEmail")]
        public string SendActivationEmail { get; set; }

        [JsonProperty("activationAccessCode")]
        public string ActivationAccessCode { get; set; }
    }
}
