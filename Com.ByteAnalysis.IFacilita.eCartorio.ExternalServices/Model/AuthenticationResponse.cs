using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Model
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }

        public DateTime DataExpiracao { get; set; }
    }
}
