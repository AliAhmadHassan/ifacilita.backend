using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Com.ByteAnalysis.IFacilita.Draft.API
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
                .UseUrls(new string[] { "http://*:5100" })
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
