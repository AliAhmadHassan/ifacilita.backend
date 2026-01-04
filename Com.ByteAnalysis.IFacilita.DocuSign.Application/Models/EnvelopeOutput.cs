using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EnvelopeOutput
    {
        public string EnvelopeId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset StatusDateTime { get; set; }

        public string Uri { get; set; }

        public List<DocumentReponse> DocumentsCallback { get; set; }
    }

    public class DocumentReponse
    {
        public DateTime DateReceived { get; set; }

        public string FileName { get; set; }

        public string Status { get; set; }

        public string Url { get; set; }
    }
}
