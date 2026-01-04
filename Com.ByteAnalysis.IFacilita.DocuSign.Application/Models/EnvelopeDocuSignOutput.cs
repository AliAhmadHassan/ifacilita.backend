using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EnvelopeDocuSignOutput
    {
        public string EnvelopeId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset StatusDateTime { get; set; }

        public string Uri { get; set; }
    }
}
