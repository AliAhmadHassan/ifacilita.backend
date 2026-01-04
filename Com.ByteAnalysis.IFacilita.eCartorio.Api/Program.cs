using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Api
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.{env}.json", true)
             .AddJsonFile("hosting.json", false, true);

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
               .UseKestrel()
               .UseStartup<Startup>()
               .Build();

            Console.Title = "[API] - eCartório RJ";
            LocalLog.WriteLogStart(configuration, "[API] eCartório RJ", "Certidões");

            host.Run();
        }

    }
}
