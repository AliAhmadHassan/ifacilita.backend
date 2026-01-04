using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi.Impl
{
    public class ClientPropertyRegistrationDataApi: IClientPropertyRegistrationDataApi
    {
        private readonly IConfiguration _configuration;

        public ClientPropertyRegistrationDataApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PropertyRegistrationDataRequestResponse Get(string id)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);
            var response = client.GetAsync(_configuration["Apis:PropertyRegistrationData:UrlApi"] + "/CertidaoCadastraisSp/" + id).Result;
            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = JsonConvert.DeserializeObject<PropertyRegistrationDataRequestResponse>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }

        public PropertyRegistrationDataRequestResponse Post(PropertyRegistrationDataRequestResponse body)
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            using (HttpClient client = new HttpClient(clientHandler))
            {
                var response = client.PostAsync(_configuration["Apis:PropertyRegistrationData:UrlApi"] + "/CertidaoCadastraisSp", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseBody = JsonConvert.DeserializeObject<PropertyRegistrationDataRequestResponse>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
        }
    }
}
