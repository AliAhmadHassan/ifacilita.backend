using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Rpa.ApiClient
{
    public class CertificatedApiClient
    {
        private HttpClient httpClient;
        private IConfiguration _configuration;

        public CertificatedApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ResumeOrderModel>> GetPendingAsync()
        {
            IEnumerable<ResumeOrderModel> listReturn = new List<ResumeOrderModel>();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = $"{_configuration["Api:UrlBase"]}/{_configuration["Api:Get"]}";
                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<ResumeOrderModel>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listReturn;
        }

        public async Task<ResumeOrderModel> GetCurrentAsync(string document, DateTime date)
        {
            ResumeOrderModel listReturn = new ResumeOrderModel();

            try
            {
                using (httpClient = new HttpClient())
                {
                    var url = $"{_configuration["Api:UrlBase"]}/{_configuration["Api:GetCurrent"]}";

                    var httpQueryString = HttpUtility.ParseQueryString(string.Empty);
                    httpQueryString.Add("document", document);
                    httpQueryString.Add("date", date.ToString("yyyy-MM-dd"));

                    var response = await httpClient.GetAsync(url + $"?{httpQueryString}");

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<ResumeOrderModel>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return listReturn;
        }


        public async Task PostAsync(ResumeOrderModel certificate)
        {
            using (httpClient = new HttpClient())
            {
                var url = _configuration["Api:UrlBase"] + "/" + _configuration["Api:Post"];

                var jsonContent = JsonConvert.SerializeObject(certificate);
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

        public async Task PutAsync(ResumeOrderModel certificate)
        {
            using (httpClient = new HttpClient())
            {
                var url = _configuration["Api:UrlBase"] + "/" + _configuration["Api:Put"];

                var jsonContent = JsonConvert.SerializeObject(certificate);
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
