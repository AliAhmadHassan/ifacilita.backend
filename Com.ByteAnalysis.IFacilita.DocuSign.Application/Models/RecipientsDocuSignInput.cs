using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    [JsonObject(Title = "recipients")]
    public class RecipientsDocuSignInput
    {
        public SignerDocuSignInput[] Signers { get; set; }
    }
}
