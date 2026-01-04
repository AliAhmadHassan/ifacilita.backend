using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Api
{
    public static class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
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

            Console.Title = "[API] Light - Troca de Titularidade";
            host.Run();

        }
    }
}
