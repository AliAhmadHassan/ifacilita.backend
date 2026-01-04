using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class GroupList
    {
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("permissionProfileId")]
        public string PermissionProfileId { get; set; }

        [JsonProperty("groupType")]
        public string GroupType { get; set; }

        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }
}
