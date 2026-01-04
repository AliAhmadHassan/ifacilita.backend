using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class Recipients
    {
        [JsonProperty("signers")]
        public Signer[] Signers { get; set; }
    }
}
