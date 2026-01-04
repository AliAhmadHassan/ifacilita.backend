using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    [JsonObject(Title = "tab")]
    public class TabDocuSignInput
    {
        public string AnchorString { get; set; }

        public long AnchorYOffset { get; set; }

        public string FontSize { get; set; }

        public string Name { get; set; }

        public long RecipientId { get; set; }

        public string TabLabel { get; set; }
    }
}
