using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi.Impl
{
    public class ClientIptuDebtsApi : IClientIptuDebtsApi
    {
        private readonly IConfiguration _configuration;

        public ClientIptuDebtsApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IptuDebtsRequestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:IptuDebts:UrlApi"] + "/IptuDebit/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<IptuDebtsRequestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public IptuDebtsRequestResponse Post(IptuDebtsRequestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:IptuDebts:UrlApi"] + "/IptuDebit", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<IptuDebtsRequestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
