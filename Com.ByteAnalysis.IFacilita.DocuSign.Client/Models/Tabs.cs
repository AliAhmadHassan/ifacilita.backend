using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class Tabs
    {
        [JsonProperty("dateSignedTabs")]
        public Tab[] DateSignedTabs { get; set; }

        [JsonProperty("fullNameTabs")]
        public Tab[] FullNameTabs { get; set; }

        [JsonProperty("signHereTabs")]
        public SignHereTab[] SignHereTabs { get; set; }
    }
}
