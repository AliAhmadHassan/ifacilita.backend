using Com.ByteAnalysis.IFacilita.Mcri.Model;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoboAntiCaptchaExternalService
{
    public class CriClient
    {
        private readonly ApiConfigs _apiConfigs;
        private HttpClient httpClient;

        public CriClient(ApiConfigs apiConfigs)
        {
            _apiConfigs = apiConfigs;
        }

        public async Task<IEnumerable<CriModel>> GetPendingAsync()
        {
            IEnumerable<CriModel> listReturn = new List<CriModel>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = _apiConfigs.UrlBase + "/" + _apiConfigs.Get;
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<CriModel>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listReturn;
        }

        public async Task<IEnumerable<CriModel>> GetPendingGuideAsync()
        {
            IEnumerable<CriModel> listReturn = new List<CriModel>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = _apiConfigs.UrlBase + "/" + _apiConfigs.GetPendingGuide;
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<CriModel>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listReturn;
        }

        public async Task PostAsync(CriModel guideRequest)
        {
            using (httpClient = new HttpClient())
            {
                var url = _apiConfigs.UrlBase + "/" + _apiConfigs.Post;

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

        public async Task PutAsync(CriModel guideRequest)
        {
            using (httpClient = new HttpClient())
            {
                var url = _apiConfigs.UrlBase + "/" + _apiConfigs.Put;

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
