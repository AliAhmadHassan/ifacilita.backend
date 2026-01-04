using Com.ByteAnalysis.IFacilita.DocuSign.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Client.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Api
{
    public class DocuSignApi<T> : IDocuSignApi<T>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private IAuthenticationService _authenticationService;
        private string _token;

        public DocuSignApi(
            IHttpClientFactory httpClient,
            IAuthenticationService authenticationService,
            IConfiguration configuration)
        {
            _httpClient = httpClient.CreateClient();
            _configuration = configuration;
            _authenticationService = authenticationService;

            FillTokenAsync().GetAwaiter().GetResult();
        }

        private async Task<bool> FillTokenAsync()
        {
            var auth = await _authenticationService.GetLastAsync();
            if (auth != null)
                _token = auth.AccessToken;

            return await Task.FromResult(true);
        }

        public async Task<T> AuthenticationAsync()
        {
            var urlAuthentication = $"{_configuration["DocuSign:UrlAuthentication"]}?" +
                $"response_type={_configuration["DocuSign:ResponseType"]}&" +
                $"scope={_configuration["DocuSign:Scope"]}&" +
                $"client_id={_configuration["DocuSign:IntegrationKey"]}&" +
                $"redirect_uri={_configuration["DocuSign:RedirectUri"]}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(urlAuthentication),
            };

            var response = await _httpClient.SendAsync(request);
            return default(T);
        }

        public async Task<T> AuthenticationCodeAsync()
        {
            var urlAuthentication = $"https://account-d.docusign.com/oauth/auth" +
                $"?response_type=code" +
                $"&scope=signature" +
                $"&client_id={_configuration["DocuSign:IntegrationKeyCode"]}" +
                $"&redirect_uri={_configuration["DocuSign:RedirectUri"]}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(urlAuthentication),
            };

            var response = await _httpClient.SendAsync(request);
            var resp = await response.Content.ReadAsStringAsync();

            return default(T);
        }

        public async Task<bool> GenerateTokenAccess()
        {
            var auth = await _authenticationService.GetLastAsync();

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("grant_type", "authorization_code");
            queryString.Add("code", auth.Code);

            var url = $"https://account-d.docusign.com/oauth/token?{queryString}";

            var toBase64 = $"{_configuration["DocuSign:IntegrationKeyCode"]}:{_configuration["DocuSign:SecretKey"]}";
            var userApplication64 = ASCIIEncoding.ASCII.GetBytes(toBase64);
            var userApplicationString = Convert.ToBase64String(userApplication64);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers = {
                    {"Authorization","Basic " + userApplicationString}
                },
            };

            var response = await _httpClient.SendAsync(request);
            var responseData = JsonConvert.DeserializeObject<AuthenticationCodeResponse>(await response.Content.ReadAsStringAsync());

            if (response.IsSuccessStatusCode)
            {
                auth.AccessToken = responseData.access_token;
                auth.RefreshToken = responseData.refresh_token;
                auth.ExpireIn = responseData.expires_in;
                auth.TokenType = responseData.token_type;

                await _authenticationService.UpdateAsync(auth.Id, auth);
            }
            else
            {
                throw new BadRequestException(await response.Content.ReadAsStringAsync());
            }

            return await Task.FromResult(true);
        }

        public Task<bool> Delete(params string[] route)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(params string[] route)
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> GetAsync(params string[] route)
        {
            var url = string.Join('/', route);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Authorization", "Bearer " + _token }
                }
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            else
            {
                var respStr = await response.Content.ReadAsStringAsync();
                throw new Exception($"DocuSign respondeu: {response.ReasonPhrase} - {respStr}");
            }
        }

        public async Task<T> PostAsync<Y>(Y obj, params string[] route)
        {
            var url = string.Join('/', route);

            var jsonContent = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "Authorization", "Bearer " + _token }
                },
                Content = content
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            else
            {
                var respStr = await response.Content.ReadAsStringAsync();
                throw new Exception($"DocuSign respondeu: {response.ReasonPhrase} - {respStr}");
            }

        }

        public Task<T> Put(T obj, params string[] route)
        {
            throw new System.NotImplementedException();
        }
    }
}
