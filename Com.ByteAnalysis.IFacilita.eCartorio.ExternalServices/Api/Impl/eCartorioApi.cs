using Com.ByteAnalysis.IFacilita.eCartorio.Application.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Api.Impl
{
    public class eCartorioApi : IeCartorioApi, IDisposable
    {
        private readonly ICache<AuthenticationResponseeCartorio> _cache;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClient;
        private readonly IAuthenticationService _authenticationService;

        private Authentication authentication;
        private readonly string cache_authentication;
        private HttpClientHandler clientHandler;

        public eCartorioApi(
            IHttpClientFactory httpClient,
            IConfiguration configuration,
            IAuthenticationService authenticationService,
            ICache<AuthenticationResponseeCartorio> cache)
        {
            _cache = cache;
            _configuration = configuration;
            _httpClient = httpClient;
            _authenticationService = authenticationService;

            cache_authentication = "cache_authentication";

        }

        private async Task<bool> LoadTokenAsync()
        {
            var client = _configuration["eCartorio:ClientId"];
            var key = _configuration["eCartorio:ClientKey"];

            var cache_auth = await _cache.GetAsync(cache_authentication);

            if (cache_auth != null && cache_auth.ExpirationDate.AddMinutes(20) >= DateTime.Now)
            {
                var _authResponse = await AuthenticationAsync(new AuthenticationRequest() { ClienteId = client, ClienteChave = key });
                _cache.Remove(cache_authentication);

                await _cache.AddAsync(cache_authentication, new AuthenticationResponseeCartorio() { ExpirationDate = _authResponse.DataExpiracao, Token = _authResponse.Token }, _authResponse.DataExpiracao);
                await _authenticationService.CreateAsync(new Authentication() { AccessToken = _authResponse.Token, ExpirationDate = _authResponse.DataExpiracao });
            }
            else
            {
                var _authResponse = await AuthenticationAsync(new AuthenticationRequest() { ClienteId = client, ClienteChave = key });

                await _cache.AddAsync(cache_authentication, new AuthenticationResponseeCartorio() { ExpirationDate = _authResponse.DataExpiracao, Token = _authResponse.Token }, _authResponse.DataExpiracao);
                await _authenticationService.CreateAsync(new Authentication() { AccessToken = _authResponse.Token, ExpirationDate = _authResponse.DataExpiracao });
            }

            return await Task.FromResult(true);
        }

        public async Task<AuthenticationResponse> AuthenticationAsync(AuthenticationRequest auth)
        {
            var route = _configuration["eCartorio:UrlBase"] + $"/Autenticacao/GerarToken";
            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var req = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(route),
                    Content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(req);
                if (!response.IsSuccessStatusCode)
                    throw new eCartorioException("Não foi possível realizar a autenticação no eCartorio. " + await response.Content.ReadAsStringAsync());

                return JsonConvert.DeserializeObject<AuthenticationResponse>(await response.Content.ReadAsStringAsync());
            }

        }

        public Task<T> DeleteAsync<T>(string routeController, NameValueCollection query)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string routeController, NameValueCollection query)
        {
            var rqMsg = await PrepareHttpRequestMessage(routeController, HttpMethod.Get, query);

            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.SendAsync(rqMsg);
                if (!response.IsSuccessStatusCode)
                    throw new eCartorioException(await response.Content.ReadAsStringAsync());

                var r = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(r);
            }
        }

        public async Task<string> GetStringAsync(string routeController, NameValueCollection query)
        {
            var rqMsg = await PrepareHttpRequestMessage(routeController, HttpMethod.Get, query);

            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.SendAsync(rqMsg);
                if (!response.IsSuccessStatusCode)
                    throw new eCartorioException(await response.Content.ReadAsStringAsync());

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<T> PostAsync<T, Y>(string routeController, Y content, NameValueCollection query)
        {
            var rqMsg = await PrepareHttpRequestMessage(routeController, HttpMethod.Post, query);

            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            var json = JsonConvert.SerializeObject(content);

            using (var client = new HttpClient(clientHandler))
            {
                var r = JsonConvert.SerializeObject(content);
                rqMsg.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(rqMsg);
                if (!response.IsSuccessStatusCode)
                {
                    var responseTxt = await response.Content.ReadAsStringAsync();
                    throw new eCartorioException(await response.Content.ReadAsStringAsync());
                }
                    

                var resp = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<T> PutAsync<T, Y>(string routeController, Y content, NameValueCollection query)
        {
            var rqMsg = await PrepareHttpRequestMessage(routeController, HttpMethod.Put, query);

            using var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var r = JsonConvert.SerializeObject(content);
                rqMsg.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await client.SendAsync(rqMsg);
                if (!response.IsSuccessStatusCode)
                    throw new eCartorioException(await response.Content.ReadAsStringAsync());

                var resp = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
        }

        private async Task<HttpRequestMessage> PrepareHttpRequestMessage(string url, HttpMethod httpMethod, NameValueCollection queryString)
        {
            var cache_auth = await _cache.GetAsync(cache_authentication);
            if (cache_auth == null)
            {
                await LoadTokenAsync();
                cache_auth = await _cache.GetAsync(cache_authentication);
            }

            return new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = queryString != null ? new Uri($"{url}?{queryString}") : new Uri(url),
                Headers = {
                    {"Authorization", "Bearer " + cache_auth.Token}
                }
            };
        }

        public void Dispose()
        {
            clientHandler?.Dispose();
            clientHandler = null;
            GC.SuppressFinalize(this);
        }
    }
}
