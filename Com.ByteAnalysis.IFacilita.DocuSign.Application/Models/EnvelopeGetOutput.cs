using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EnvelopeGetOutput
    {
        public bool AllowMarkup { get; set; }

        public bool AutoNavigation { get; set; }

        public string BrandId { get; set; }

        public string CertificateUri { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

        public string CustomFieldsUri { get; set; }

        public string DocumentsCombinedUri { get; set; }

        public string DocumentsUri { get; set; }

        public string EmailSubject { get; set; }

        public bool EnableWetSign { get; set; }

        public string EnvelopeId { get; set; }

        public bool EnvelopeIdStamping { get; set; }

        public string EnvelopeUri { get; set; }

        public DateTimeOffset InitialSentDateTime { get; set; }

        public bool Is21CfrPart11 { get; set; }

        public bool IsSignatureProviderEnvelope { get; set; }

        public DateTimeOffset LastModifiedDateTime { get; set; }

        public string NotificationUri { get; set; }

        public string PurgeState { get; set; }

        public string RecipientsUri { get; set; }

        public DateTimeOffset SentDateTime { get; set; }

        public string Status { get; set; }

        public DateTimeOffset StatusChangedDateTime { get; set; }

        public string TemplatesUri { get; set; }
    }
}
