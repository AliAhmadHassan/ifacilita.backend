using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class AuthenticationDocuSignInput
    {
        public string AccessToken { get; set; }

        public DateTime DateGeneration { get; set; }

        public int ExpiresIn { get; set; }

        public string TokenType { get; set; }
    }
}
