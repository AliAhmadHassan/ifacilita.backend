using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    [JsonObject(Title = "signHereTab")]
    public class SignHereTabDocuSignInput
    {
        public string AnchorString { get; set; }

        public string AnchorUnits { get; set; }

        public long AnchorXOffset { get; set; }

        public long AnchorYOffset { get; set; }

        public string Name { get; set; }

        public bool Optional { get; set; }

        public long RecipientId { get; set; }

        public long ScaleValue { get; set; }

        public string TabLabel { get; set; }
    }
}
