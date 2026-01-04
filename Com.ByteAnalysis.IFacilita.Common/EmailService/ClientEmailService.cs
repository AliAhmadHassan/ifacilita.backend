using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Common.EmailService
{
    public class ClientEmailService : IClientEmailService
    {
        private readonly IConfiguration _configuration;

        public ClientEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendMail(IList<Tuple<string, string>> emailTo, string subject, string message, string[] pathAttachments, List<object> otherFields, string messageType = "Wellcome")
        {
            using var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            List<object> to = new List<object>();
            foreach (var email in emailTo)
                to.Add(new { name = email.Item1, email = email.Item2 });

            using (HttpClient httpClient = new HttpClient(clientHandler))
            {
                var bodyPost = new
                {
                    to,
                    messageType = messageType,
                    otherFields
                };

                var responseEmail = httpClient.PostAsync(_configuration["ApiEmailService"], new StringContent(JsonConvert.SerializeObject(bodyPost), Encoding.UTF8, "application/json")).Result;
                if (responseEmail.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
    }
}
