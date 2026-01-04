using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway.Impl
{
    public class ClientApi : IClientApi
    {
        public ClientApi()
        {

        }

        public Task<T> DeleteAsync<T>(string routeController, string token, NameValueCollection query)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string routeController, string token, NameValueCollection query)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Add("Authorization", token);

                var response = await client.GetAsync($"{routeController}?{query}");
                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                return responseObj;
            }
        }

        public async Task<T> PostAsync<T, Y>(string routeController, Y content, string token, NameValueCollection query)
        {
            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Add("Authorization", token);

                var response = await client.PostAsync(routeController, new StringContent(System.Text.Json.JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                
                return responseObj;
            }
        }

        public async Task<T> PutAsync<T, Y>(string routeController, Y content, string token, NameValueCollection query)
        {
            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic NWZmYjRhOWNjNTJiZWU3YjcwOGI1Nzg2Y2M5ZjBjYTE6");

                var response = await client.PutAsync(routeController, new StringContent(System.Text.Json.JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                return responseObj;
            }
        }
    }
}
