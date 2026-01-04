using Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers;
using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class Document
    {
        [JsonProperty("documentBase64")]
        public string DocumentBase64 { get; set; }

        [JsonProperty("documentId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long DocumentId { get; set; }

        [JsonProperty("fileExtension")]
        public string FileExtension { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
