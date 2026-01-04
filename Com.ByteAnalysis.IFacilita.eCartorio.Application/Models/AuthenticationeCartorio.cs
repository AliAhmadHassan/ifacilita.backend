using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models
{
    public class AuthenticationResponseeCartorio
    {
        public string Token { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
