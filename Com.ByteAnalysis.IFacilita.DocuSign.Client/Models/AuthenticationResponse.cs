using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }

        public DateTime DateGeneration { get; set; }

        public int ExpiresIn { get; set; }

        public string TokenType { get; set; }

    }
}
