using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Rpa.ApiClient
{
    public class SearchProtestClientApi
    {
        private HttpClient httpClient;
        private IConfiguration _configuration;

        public SearchProtestClientApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<RequestSearchProtestModel>> GetPendingAsync()
        {
            IEnumerable<RequestSearchProtestModel> listReturn = new List<RequestSearchProtestModel>();
            var url = $"{_configuration["Api:UrlBase"]}/{_configuration["Api:Get"]}";

            try
            {
                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (httpClient = new HttpClient(clientHandler))
                {

                    var response = await httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    listReturn = JsonConvert.DeserializeObject<List<RequestSearchProtestModel>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");

                System.IO.File.AppendAllLines($"Logs/eCartorioSPAPI-{DateTime.Now.ToString("dd-mm-yyyy-HH")}.log",
                       new[] {
                            $"Momento do Erro: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                            $"--------------------------------------------",
                            $"Método: GetPendingAsync()",
                            $"Erro Ocorrido: {ex.Message}",
                            $"Url Get: {url}",
                            $"*********************************************************************\n"
                       });

                return null;
            }

            return listReturn;
        }

        public async Task PostAsync(RequestSearchProtestModel guideRequest)
        {
            var url = _configuration["Api:UrlBase"] + "/" + _configuration["Api:Post"];

            try
            {
                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (httpClient = new HttpClient(clientHandler))
                {


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
                if (!System.IO.Directory.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");

                System.IO.File.AppendAllLines($"Logs/eCartorioSPAPI-{DateTime.Now.ToString("dd-mm-yyyy-HH")}.log",
                      new[] {
                            $"Momento do Erro: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                            $"--------------------------------------------",
                            $"Método: PostAsync(RequestSearchProtestModel)",
                            $"Erro Ocorrido: {ex.Message}",
                            $"Url Get: {url}",
                            $"Payload: {JsonConvert.SerializeObject(guideRequest)}",
                            $"*********************************************************************\n"
                      });
            }
        }

        public async Task PutAsync(RequestSearchProtestModel guideRequest)
        {
            var url = _configuration["Api:UrlBase"] + "/" + _configuration["Api:Put"];

            try
            {
                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (httpClient = new HttpClient(clientHandler))
                {
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
                if (!System.IO.Directory.Exists("Logs"))
                    System.IO.Directory.CreateDirectory("Logs");

                System.IO.File.AppendAllLines($"Logs/eCartorioSPAPI-{DateTime.Now.ToString("dd-mm-yyyy-HH")}.log",
                     new[] {
                          $"Momento do Erro: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}",
                            $"--------------------------------------------",
                            $"Método: PostAsync(RequestSearchProtestModel)",
                            $"Erro Ocorrido: {ex.Message}",
                            $"Url Get: {url}",
                            $"Payload: {JsonConvert.SerializeObject(guideRequest)}",
                            $"*********************************************************************\n"
                     });
            }
        }
    }
}
