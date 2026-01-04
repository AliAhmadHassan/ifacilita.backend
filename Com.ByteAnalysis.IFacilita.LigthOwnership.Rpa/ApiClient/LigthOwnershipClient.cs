using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Rpa.ApiClient
{
    public class LigthOwnershipClient
    {
        private HttpClient httpClient;
        private IConfiguration _configuration;

        public LigthOwnershipClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<OwnershipModel>> GetPendingAsync()
        {
            IEnumerable<OwnershipModel> listReturn = new List<OwnershipModel>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = $"{_configuration["Api:UrlBase"]}/{_configuration["Api:Get"]}";
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<OwnershipModel>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listReturn;
        }

        public async Task PostAsync(OwnershipModel guideRequest)
        {
            using (httpClient = new HttpClient())
            {
                var url = _configuration["Api:UrlBase"] + "/" +  _configuration["Api:Post"];

                var jsonContent = JsonConvert.SerializeObject(guideRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = content
                };

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Houve uma falha ao tentar atualizar a guia de requisição.");
            }
        }

        public async Task PutAsync(OwnershipModel guideRequest)
        {
            using (httpClient = new HttpClient())
            {
                var url = _configuration["Api:UrlBase"] + "/" + _configuration["Api:Put"];

                var jsonContent = JsonConvert.SerializeObject(guideRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = content
                };

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Houve uma falha ao tentar atualizar a guia de requisição.");
            }
        }
    }
}
