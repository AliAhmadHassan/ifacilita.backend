using Com.ByteAnalysis.IFacilita.Service;
using Newtonsoft.Json;
using RoboAntiCaptchaExternalService.Interfaces;
using RoboAntiCaptchaModel.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoboAntiCaptchaExternalService
{
    public class FacilitaClient : IFacilitaClient
    {
        private readonly ApiConfigs _apiConfigs;
        private HttpClient httpClient;

        public FacilitaClient(ApiConfigs apiConfigs)
        {
            _apiConfigs = apiConfigs;
        }

        public async Task<IEnumerable<GuideRequest>> GetPendingAsync()
        {
            IEnumerable<GuideRequest> listReturn = new List<GuideRequest>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = _apiConfigs.UrlBase + "/" + _apiConfigs.Get;
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<GuideRequest>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                await WriteLog(ex.Message);
                return null;
            }

            return listReturn;
        }

        public async Task<IEnumerable<GuideRequest>> GetPendingGuideAsync()
        {
            IEnumerable<GuideRequest> listReturn = new List<GuideRequest>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = _apiConfigs.UrlBase + "/" + _apiConfigs.GetPendingGuide;
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<GuideRequest>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                await WriteLog(ex.Message);
                return null;
            }

            return listReturn;
        }

        public async Task PostAsync(GuideRequest guideRequest)
        {
            try
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
            catch (Exception ex)
            {
                await WriteLog(ex.Message);
            }
        }

        public async Task PutAsync(GuideRequest guideRequest)
        {
            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = _apiConfigs.UrlBase + "/" + _apiConfigs.Put;
                    //var url = guideRequest.UrlCallback.Replace("{guid}", guideRequest.Id);

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
            catch (Exception ex)
            {
                await WriteLog(ex.Message);
            }
        }

        private async Task<bool> WriteLog(string message)
        {
            try
            {
              
                var msg = $"[{DateTime.Now}] - [FacilitaClient] - {message}";
                await File.AppendAllLinesAsync($"{DateTime.Now.ToString("dd-MM-yyyyHH")}.log", new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }

            return true;
        }
    }
}
