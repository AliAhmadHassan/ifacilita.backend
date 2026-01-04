namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EnvelopeDocuSignInput
    {
        public DocumentDocuSignInput[] Documents { get; set; }

        public RecipientsDocuSignInput[] Recipients { get; set; }

        public EventNotificationDocuSignInput EventNotification { get; set; }

        public string EmailSubject { get; set; }

        public string Status { get; set; }
    }
}
