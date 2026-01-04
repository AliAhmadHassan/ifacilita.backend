using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    [JsonObject(Title = "tabs")]
    public class TabsDocuSignInput
    {
        public TabDocuSignInput[] DateSignedTabs { get; set; }

        public TabDocuSignInput[] FullNameTabs { get; set; }

        public SignHereTabDocuSignInput[] SignHereTabs { get; set; }
    }
}
