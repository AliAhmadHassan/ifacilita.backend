using System;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Com.ByteAnalysis.IFacilita.Common.Extensions
{
    public static class IfacilitaExtensions
    {
        public static int ToInt32(this string value)
        {
            return Convert.ToInt32(value);
        }

        public static void RegisterLogStart(this WebHostBuilder host, IConfiguration configuration)
        {
            LocalLog.WriteLogStart(configuration, "[API] IptuDebts", "Débitos do IPTU");
        }
    }
}
