using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class WorkAddress
    {
        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateOrProvince")]
        public string StateOrProvince { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
