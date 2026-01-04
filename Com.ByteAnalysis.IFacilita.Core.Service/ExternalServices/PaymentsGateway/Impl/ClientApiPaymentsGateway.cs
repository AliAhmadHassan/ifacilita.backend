using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway;
using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway;
using Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway.SessionAuthenticationToken;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway.Impl
{
    public class ClientApiPaymentsGateway : IClientApiPaymentsGateway
    {
        private readonly IConfiguration _configuration;
        private readonly IClientApi _clientApi;
        private readonly string urlBase;

        private NameValueCollection query;


        public ClientApiPaymentsGateway(IConfiguration configuration, IClientApi clientApi)
        {
            _configuration = configuration;
            _clientApi = clientApi;

            urlBase = _configuration["PaymentsGateway:UrlBase"];
            query = HttpUtility.ParseQueryString(string.Empty);
        }

        public async Task<AuthenticationResponseDto> AuthenticationAsync()
        {
            if (!await TokenManagerService.IsValidTokenAsync())
            {
                var request = new AuthenticationRequestDto()
                {
                    AccessKey = _configuration["PaymentsGateway:Password"],
                    Login = _configuration["PaymentsGateway:User"]
                };

                var response = await _clientApi.PostAsync<AuthenticationResponseDto, AuthenticationRequestDto>($"{urlBase}/Authentications", request, null, query);
                TokenManagerService.Auth = response;
            }

            return TokenManagerService.Auth;
        }

        public async Task<ClientDto> CreateClientAsync(ClientDto client)
        {
            _ = await AuthenticationAsync();
            var result = await _clientApi.PostAsync<ClientDto, ClientDto>($"{urlBase}/client", client, $"Bearer {TokenManagerService.Auth.AccessToken}", query);
            return result;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoice)
        {
            _ = await AuthenticationAsync();

            var result = await _clientApi.PostAsync<InvoiceDto, InvoiceDto>($"{urlBase}/invoice", invoice, $"Bearer {TokenManagerService.Auth.AccessToken}", query);
            return result;
        }
    }
}
