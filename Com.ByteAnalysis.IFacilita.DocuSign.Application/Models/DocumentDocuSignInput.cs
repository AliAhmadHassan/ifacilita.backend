namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class DocumentDocuSignInput
    {
        public string DocumentBase64 { get; set; }

        public long DocumentId { get; set; }

        public string FileExtension { get; set; }

        public string Name { get; set; }
    }
}
