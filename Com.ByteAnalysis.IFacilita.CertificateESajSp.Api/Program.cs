using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Api
{
    public static class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.{env}.json", true);

            configuration = builder.Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                .ConfigureLogging(b =>
                {
                    b.SetMinimumLevel(LogLevel.Information);
                    b.AddConfiguration(configuration);
                    b.AddConsole();
                })
                .UseKestrel((ctx, opt) =>
                {
                    opt.Configure(ctx.Configuration.GetSection("Kestrel"))
                    .Endpoint("HTTPS", lst =>
                    {
                        lst.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    });
                })
                .UseStartup<Startup>()
                .Build();

            Console.Title = "[API] DefectsDefined - Defeitos Ajuizados";
            LocalLog.WriteLogStart(configuration, "[API] DefectsDefined", "Defeitos Ajuizados");
            host.Run();
        }
    }
}
