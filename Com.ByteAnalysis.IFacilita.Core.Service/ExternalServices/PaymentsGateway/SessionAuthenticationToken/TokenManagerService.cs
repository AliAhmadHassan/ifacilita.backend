using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway;
using System;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway.SessionAuthenticationToken
{
    public static class TokenManagerService
    {
        private static AuthenticationResponseDto auth;

        public static AuthenticationResponseDto Auth
        {
            get { return auth; }
            set { auth = value; }
        }

        public static Task<bool> IsValidTokenAsync()
        {
            //Verifica se já foi autenticado
            if (auth == null)
                return Task.FromResult(false);

            //Verifica se o agora - 30 minutos é menor que a expiração para poder gerar um novo token
            if (auth.Expiration <= DateTime.Now.AddMinutes(-30))
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

    }
}
