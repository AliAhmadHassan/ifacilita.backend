using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi.Impl
{
    public class DefectsDefinedClientApi: IDefectsDefinedClientApi
    {
        private readonly IConfiguration _configuration;

        public DefectsDefinedClientApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CertificateDefectsDefinedEntityRequestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:DefectsDefined:UrlApi"] + "/Certificate/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<CertificateDefectsDefinedEntityRequestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public CertificateDefectsDefinedEntityRequestResponse Post(CertificateDefectsDefinedEntityRequestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:DefectsDefined:UrlApi"] + "/Certificate", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<CertificateDefectsDefinedEntityRequestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
