using Com.ByteAnalysis.IFacilita.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api.HealthCheck
{
    public class ECartorioHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public ECartorioHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
            HealthCheckService.TimeOutInMinutes = _configuration["eCartorio:TimeOutHealthCheck"].ToInt32();
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (HealthCheckService.LastCheck > DateTime.Now)
                    return HealthCheckService.LastResult;

                #region MyRegion
                    //var urlHost = new Uri(_configuration["eCartorio:UrlBase"] + "/index.html");
                    //Ping ping = new Ping();
                    //PingReply result = await ping.SendPingAsync(urlHost.Host);

                    //var res = result.Status; 
                    #endregion

                var urlHost = new Uri(_configuration["eCartorio:UrlBase"] + "/index.html");

                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    var response = await httpClient.GetAsync(urlHost);

                    if (response.IsSuccessStatusCode)
                        HealthCheckService.LastResult = HealthCheckResult.Healthy("Available service");
                    else
                        HealthCheckService.LastResult = new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
                }

                HealthCheckService.LastCheck = DateTime.Now.AddMinutes(HealthCheckService.TimeOutInMinutes);
                return HealthCheckService.LastResult;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - [ERR] - {ex.Message}");
                return new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
            }
        }
    }
}
