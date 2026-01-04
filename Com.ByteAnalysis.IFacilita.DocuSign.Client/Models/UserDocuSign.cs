using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class UserDocuSign
    {
        public UserDocuSign()
        {
            NewUsers = new List<UserResponse>();
        }

        [JsonProperty("newUsers")]
        public List<UserResponse> NewUsers { get; set; }
    }
}
