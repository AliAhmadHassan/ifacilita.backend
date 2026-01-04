using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Api.HealthCheck
{
    public static class HealthCheckManager
    {
        private static List<HealthCheckService> checkServices;

        public static List<HealthCheckService> CheckServices { get => checkServices; set => checkServices = value; }

        public static async Task<bool> AddServiceAsync(HealthCheckService service)
        {
            if (checkServices == null)
                checkServices = new List<HealthCheckService>();

            if (await ExistServiceAsync(service.ServiceName))
                return await Task.FromResult(false);
            
            checkServices.Add(service);

            return await Task.FromResult(true);
        }

        public static async Task<HealthCheckService> UpdateServiceAsync(HealthCheckService service)
        {
            if (checkServices == null)
                checkServices = new List<HealthCheckService>();

            if (!await ExistServiceAsync(service.ServiceName))
                return null;

            var updateService = checkServices.FirstOrDefault(x => x.ServiceName.Equals(service.ServiceName));
            
            updateService.LastResult = service.LastResult;
            updateService.LastCheck = DateTime.Now.AddMinutes(updateService.TimeOutInMinutes);

            return await Task.FromResult(updateService);
        }

        public static async Task<bool> ExistServiceAsync(string serviceName)
        {
            if (checkServices == null)
                checkServices = new List<HealthCheckService>();

            return await Task.FromResult(checkServices.Any(x => x.ServiceName.Equals(serviceName)));
        }

        public static async Task<bool> RemoveServiceAsync(string serviceName)
        {
            if (checkServices == null)
                return await Task.FromResult(false);

            if (!await ExistServiceAsync(serviceName))
                return await Task.FromResult(false);

            var removedService = checkServices.FirstOrDefault(x => x.ServiceName.Equals(serviceName));
            checkServices.Remove(removedService);

            return await Task.FromResult(false);
        }

        public static async Task<HealthCheckService> GetServiceAsync(string serviceName)
        {
            if (checkServices == null)
                return null;

            if (!await ExistServiceAsync(serviceName))
                return null;

            return await Task.FromResult(checkServices.FirstOrDefault(x => x.ServiceName.Equals(serviceName)));
        }
    }

    public class HealthCheckService
    {
        private string serviceName;

        private int timeOutInMinutes;

        private DateTime lastCheck;

        private HealthCheckResult lastResult;

        public int TimeOutInMinutes { get => timeOutInMinutes; set => timeOutInMinutes = value; }

        public DateTime LastCheck { get => lastCheck; set => lastCheck = value; }

        public HealthCheckResult LastResult { get => lastResult; set => lastResult = value; }

        public string ServiceName { get => serviceName; set => serviceName = value; }
    }
}
