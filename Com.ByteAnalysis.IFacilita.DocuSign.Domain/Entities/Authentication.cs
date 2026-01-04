using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities
{
    public class Authentication : EntityBase
    {
        public string Code { get; set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string RefreshToken { get; set; }

        public int ExpireIn { get; set; }

        public DateTime Created { get; set; }
    }
}
