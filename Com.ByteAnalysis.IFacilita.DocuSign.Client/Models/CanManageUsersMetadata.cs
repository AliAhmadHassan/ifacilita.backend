using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class CanManageUsersMetadata
    {
        [JsonProperty("rights")]
        public string Rights { get; set; }

        [JsonProperty("uiHint")]
        public string UiHint { get; set; }

        [JsonProperty("uiType")]
        public string UiType { get; set; }

        [JsonProperty("uiOrder")]
        public string UiOrder { get; set; }

        [JsonProperty("is21CFRPart11")]
        public string Is21CFRPart11 { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }
    }
}
