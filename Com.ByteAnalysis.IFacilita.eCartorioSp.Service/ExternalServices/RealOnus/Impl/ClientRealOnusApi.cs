using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus.Impl
{
    public class ClientRealOnusApi: IClientRealOnusApi
    {
        private readonly IConfiguration _configuration;

        public ClientRealOnusApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RealOnusRequestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:RealOnus:UrlApi"] + "/OnusReal/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<RealOnusRequestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public RealOnusRequestResponse Post(RealOnusRequestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:RealOnus:UrlApi"] + "/OnusReal", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<RealOnusRequestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
