using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace Com.ByteAnalysis.IFacilita.Core.Api.HealthCheck
{
    public static class HealthCheckService
    {
        public static int TimeOutInMinutes;

        public static DateTime LastCheck;

        public static HealthCheckResult LastResult;

        public static int TimeOutInMinutesSp;

        public static DateTime LastCheckSp;

        public static HealthCheckResult LastResultSp;
    }
}
