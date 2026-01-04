using Com.ByteAnalysis.IFacilita.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.ServiceMail.Api.HealthCheck
{

    public class SiteHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public SiteHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
            HealthCheckService.TimeOutInMinutes = _configuration["TimeOutHealthCheck"].ToInt32();
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (HealthCheckService.LastCheck > DateTime.Now)
                    return HealthCheckService.LastResult;

                Ping ping = new Ping();
                PingReply result = await ping.SendPingAsync(_configuration["EmailConfiguration:SmtpServer"]);
                var res = result.Status;
               
                if(result.Status != IPStatus.Success)
                    HealthCheckService.LastResult = new HealthCheckResult(context.Registration.FailureStatus, "Unavailable service");
                else
                    HealthCheckService.LastResult = HealthCheckResult.Healthy("Available service");

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
