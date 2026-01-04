using System;

namespace Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway
{
    public class AuthenticationResponseDto
    {
        public bool Authenticated { get; set; }

        public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Expiration { get; set; }

        public string AccessToken { get; set; }

        public string Message { get; set; }

    }
}
