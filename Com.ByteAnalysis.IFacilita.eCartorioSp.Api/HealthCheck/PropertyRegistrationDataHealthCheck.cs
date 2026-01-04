using Com.ByteAnalysis.IFacilita.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Api.HealthCheck
{
    public class PropertyRegistrationDataHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        private const string serviceName = "PropertyRegistrationDataApi";
        private HealthCheckService currentHealthCheck;


        public PropertyRegistrationDataHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await HealthCheckManager.ExistServiceAsync(serviceName))
                {
                    currentHealthCheck = new HealthCheckService()
                    {
                        LastCheck = DateTime.Now,
                        LastResult = HealthCheckResult.Healthy("Available service"),
                        ServiceName = serviceName,
                        TimeOutInMinutes = _configuration["Apis:PropertyRegistrationData:TimeOutHealthCheck"].ToInt32()
                    };
                    await HealthCheckManager.AddServiceAsync(currentHealthCheck);
                }
                else
                {
                    currentHealthCheck = await HealthCheckManager.GetServiceAsync(serviceName);
                }

                if (currentHealthCheck.LastCheck > DateTime.Now)
                    return currentHealthCheck.LastResult;

                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    var urlHost = new Uri(_configuration["Apis:PropertyRegistrationData:Url"] + "/api/HealthCheck");

                    var response = await httpClient.GetAsync(urlHost);

                    if (response.IsSuccessStatusCode)
                        currentHealthCheck.LastResult = HealthCheckResult.Healthy("Available service");
                    else
                        currentHealthCheck.LastResult = new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
                }

                _ = await HealthCheckManager.UpdateServiceAsync(currentHealthCheck);
                return currentHealthCheck.LastResult;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - [ERR] - {ex.Message}");
                return new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
            }
        }
    }

    public class PropertyRegistrationDataSiteHealthCheck : IHealthCheck
    {
        
        private readonly IConfiguration _configuration;
        private const string serviceName = "PropertyRegistrationDataSite";
        private HealthCheckService currentHealthCheck;

        public PropertyRegistrationDataSiteHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!await HealthCheckManager.ExistServiceAsync(serviceName))
                {
                    currentHealthCheck = new HealthCheckService()
                    {
                        LastCheck = DateTime.Now,
                        LastResult = HealthCheckResult.Healthy("Available service"),
                        ServiceName = serviceName,
                        TimeOutInMinutes = _configuration["Apis:PropertyRegistrationData:TimeOutHealthCheck"].ToInt32()
                    };
                    await HealthCheckManager.AddServiceAsync(currentHealthCheck);
                }
                else
                {
                    currentHealthCheck = await HealthCheckManager.GetServiceAsync(serviceName);
                }

                if (currentHealthCheck.LastCheck > DateTime.Now)
                    return currentHealthCheck.LastResult;

                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    var urlHost = new Uri(_configuration["Apis:PropertyRegistrationData:Site"]);

                    var response = await httpClient.GetAsync(urlHost);

                    if (response.IsSuccessStatusCode)
                        currentHealthCheck.LastResult = HealthCheckResult.Healthy("Available service");
                    else
                        currentHealthCheck.LastResult = new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
                }

                _ = await HealthCheckManager.UpdateServiceAsync(currentHealthCheck);
                return currentHealthCheck.LastResult;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}] - [ERR] - {ex.Message}");
                return new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
            }
        }
    }
}
