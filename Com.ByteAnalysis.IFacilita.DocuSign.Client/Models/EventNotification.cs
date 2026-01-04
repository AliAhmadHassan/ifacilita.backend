using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class EventNotification
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("loggingEnabled")]
        public string LoggingEnabled { get; set; }

        [JsonProperty("requireAcknowledgment")]
        public string RequireAcknowledgment { get; set; }

        [JsonProperty("envelopeEvents")]
        public EnvelopeEvent[] EnvelopeEvents { get; set; }

        [JsonProperty("recipientEvents")]
        public RecipientEvent[] RecipientEvents { get; set; }

        [JsonProperty("useSoapInterface")]
        public string UseSoapInterface { get; set; }

        [JsonProperty("soapNameSpace")]
        public string SoapNameSpace { get; set; }

        [JsonProperty("includeCertificateWithSoap")]
        public string IncludeCertificateWithSoap { get; set; }

        [JsonProperty("signMessageWithX509Cert")]
        public string SignMessageWithX509Cert { get; set; }

        [JsonProperty("includeDocuments")]
        public string IncludeDocuments { get; set; }

        [JsonProperty("includeHMAC")]
        public string IncludeHmac { get; set; }

        [JsonProperty("includeEnvelopeVoidReason")]
        public string IncludeEnvelopeVoidReason { get; set; }

        [JsonProperty("includeTimeZone")]
        public string IncludeTimeZone { get; set; }

        [JsonProperty("includeSenderAccountAsCustomField")]
        public string IncludeSenderAccountAsCustomField { get; set; }

        [JsonProperty("includeDocumentFields")]
        public string IncludeDocumentFields { get; set; }

        [JsonProperty("includeCertificateOfCompletion")]
        public string IncludeCertificateOfCompletion { get; set; }
    }
}
