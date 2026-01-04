using System;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Document: BasicEntity
    {
        public Int32 Id { get; set; }
        public String FilePath { get; set; }
        public DateTime? Created { get; set; }
        public Int32? IdDocumentType { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}