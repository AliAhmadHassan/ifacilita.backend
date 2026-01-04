using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Api.HealthCheck
{
    public static class HealthCheckService
    {
        public static int TimeOutInMinutes;

        public static DateTime LastCheck;

        public static HealthCheckResult LastResult;
    }
}
