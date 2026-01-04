using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Com.ByteAnalysis.IFacilita.FileService.API
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

            configuration = builder.Build();

            var host = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                .UseUrls(new string[] { "http://*:5600" })
                .ConfigureLogging(b =>
                {
                    b.SetMinimumLevel(LogLevel.Error);
                    b.AddConfiguration(configuration);
                    b.AddConsole();
                })
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
