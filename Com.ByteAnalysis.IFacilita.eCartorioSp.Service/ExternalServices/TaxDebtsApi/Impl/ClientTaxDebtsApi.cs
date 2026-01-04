using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi.Impl
{
    public class ClientTaxDebtsApi: IClientTaxDebtsApi
    {
        private readonly IConfiguration _configuration;

        public ClientTaxDebtsApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TaxDebtsRequestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:TaxDebts:UrlApi"] + "/CertidaoDebitoCreditoSP/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<TaxDebtsRequestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public TaxDebtsRequestResponse Post(TaxDebtsRequestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:TaxDebts:UrlApi"] + "/CertidaoDebitoCreditoSP", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<TaxDebtsRequestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
