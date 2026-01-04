using Com.ByteAnalysis.IFacilita.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.HealthCheck
{
    public class ECartorioSpHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public ECartorioSpHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
            HealthCheckService.TimeOutInMinutesSp = _configuration["eCartorio:SP:TimeOutHealthCheck"].ToInt32();
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (HealthCheckService.LastCheckSp > DateTime.Now)
                    return HealthCheckService.LastResultSp;

                var urlHost = new Uri(_configuration["eCartorio:SP:UrlBase"] + "/swagger/index.html");

                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    var response = await httpClient.GetAsync(urlHost);

                    if (response.IsSuccessStatusCode)
                        HealthCheckService.LastResultSp = HealthCheckResult.Healthy("Available service");
                    else
                        HealthCheckService.LastResultSp = new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
                }

                HealthCheckService.LastCheckSp = DateTime.Now.AddMinutes(HealthCheckService.TimeOutInMinutes);
                return HealthCheckService.LastResultSp;

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - [ERR] - {ex.Message}");
                return new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
            }
        }
    }
}
