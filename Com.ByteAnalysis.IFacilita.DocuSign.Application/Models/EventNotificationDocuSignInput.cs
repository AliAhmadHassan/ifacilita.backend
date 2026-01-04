namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EventNotificationDocuSignInput
    {
        public string Url { get; set; }

        public string LoggingEnabled { get; set; }

        public string RequireAcknowledgment { get; set; }

        public EnvelopeEventDocuSignInput[] EnvelopeEvents { get; set; } 

        public RecipientEventDocuSignInput[] RecipientEvents { get; set; }

        public string UseSoapInterface { get; set; }

        public string SoapNameSpace { get; set; }

        public string IncludeCertificateWithSoap { get; set; }

        public string SignMessageWithX509Cert { get; set; }

        public string IncludeDocuments { get; set; }

        public string IncludeHmac { get; set; }

        public string IncludeEnvelopeVoidReason { get; set; }

        public string IncludeTimeZone { get; set; }

        public string IncludeSenderAccountAsCustomField { get; set; }

        public string IncludeDocumentFields { get; set; }

        public string IncludeCertificateOfCompletion { get; set; }
    }
}