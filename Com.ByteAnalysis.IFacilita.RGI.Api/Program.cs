using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.RGI.Api
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.{env}.json", true);

            configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MongoDB(configuration.GetValue<string>("MongoDatabaseSettings:BaseLog"), collectionName: "RGI.API")
                .CreateLogger();

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
                .UseSerilog()
                .Build();

            Console.Title = "[API] RGI - Registro de imóveis";
            LocalLog.WriteLogStart(configuration, "[API] RGI", "Registro de imóveis");

            host.Run();
        }
    }
}
