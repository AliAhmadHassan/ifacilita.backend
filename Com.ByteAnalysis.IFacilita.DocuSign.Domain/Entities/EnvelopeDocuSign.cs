using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities
{
    public class EnvelopeDocuSign : EntityBase
    {

        public string EmailSubject { get; set; }

        public string EnvelopeId { get; set; }

        public string Status { get; set; }

        public DateTimeOffset StatusDateTime { get; set; }

        public string Uri { get; set; }

        public List<SignsEnvelope> Signs { get; set; }

        public List<string> Documents { get; set; }

        public List<DocumentReponse> DocumentsResponse { get; set; }

    }

    public class SignsEnvelope
    {
        public string  Name { get; set; }

        public string Email { get; set; }
    }

    public class DocumentReponse
    {
        public DateTime DateReceived { get; set; }

        public string Status { get; set; }

        public string FileName { get; set; }

        public string Url { get; set; }
    }
}
