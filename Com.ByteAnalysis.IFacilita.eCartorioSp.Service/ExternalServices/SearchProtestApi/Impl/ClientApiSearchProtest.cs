using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Impl
{
    public class ClientApiSearchProtest : IClientApiSearchProtest
    {
        private readonly IConfiguration _configuration;

        public ClientApiSearchProtest(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CertificateSearchProtestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:SearchProtest:UrlApi"] + "/SearchProtest/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<CertificateSearchProtestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public CertificateSearchProtestResponse Post(CertificateSearchProtestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:SearchProtest:UrlApi"] + "/SearchProtest", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<CertificateSearchProtestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
