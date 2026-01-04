using Newtonsoft.Json;
using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class EnvelopeGetResponse
    {
        [JsonProperty("allowMarkup")]
        public bool AllowMarkup { get; set; }

        [JsonProperty("autoNavigation")]
        public bool AutoNavigation { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("certificateUri")]
        public string CertificateUri { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("customFieldsUri")]
        public string CustomFieldsUri { get; set; }

        [JsonProperty("documentsCombinedUri")]
        public string DocumentsCombinedUri { get; set; }

        [JsonProperty("documentsUri")]
        public string DocumentsUri { get; set; }

        [JsonProperty("emailSubject")]
        public string EmailSubject { get; set; }

        [JsonProperty("enableWetSign")]
        public bool EnableWetSign { get; set; }

        [JsonProperty("envelopeId")]
        public string EnvelopeId { get; set; }

        [JsonProperty("envelopeIdStamping")]
        public bool EnvelopeIdStamping { get; set; }

        [JsonProperty("envelopeUri")]
        public string EnvelopeUri { get; set; }

        [JsonProperty("initialSentDateTime")]
        public DateTimeOffset InitialSentDateTime { get; set; }

        [JsonProperty("is21CFRPart11")]
        public bool Is21CfrPart11 { get; set; }

        [JsonProperty("isSignatureProviderEnvelope")]
        public bool IsSignatureProviderEnvelope { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        [JsonProperty("notificationUri")]
        public string NotificationUri { get; set; }

        [JsonProperty("purgeState")]
        public string PurgeState { get; set; }

        [JsonProperty("recipientsUri")]
        public string RecipientsUri { get; set; }

        [JsonProperty("sentDateTime")]
        public DateTimeOffset SentDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusChangedDateTime")]
        public DateTimeOffset StatusChangedDateTime { get; set; }

        [JsonProperty("templatesUri")]
        public string TemplatesUri { get; set; }
    }
}
