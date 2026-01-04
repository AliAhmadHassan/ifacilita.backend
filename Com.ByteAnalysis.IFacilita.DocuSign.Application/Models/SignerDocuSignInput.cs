namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class SignerDocuSignInput
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public long RecipientId { get; set; }

        public long RoutingOrder { get; set; }

        public TabsDocuSignInput Tabs { get; set; }
    }
}
