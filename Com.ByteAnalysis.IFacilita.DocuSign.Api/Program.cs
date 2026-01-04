using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Api
{
    public class Program
    {
        private static IConfiguration configuration;
        public static void Main(string[] args)
        {
            Console.Out.WriteLine("Obtendo valor da variável de ambiente ASPNETCORE_ENVIRONMENT para EnvironmentVariableTarget.User");
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            Console.Out.WriteLine("Valor encontrado: " + env);

            if (string.IsNullOrEmpty(env))
            {
                Console.Out.WriteLine("Obtendo valor da variável de ambiente ASPNETCORE_ENVIRONMENT para EnvironmentVariableTarget.Machine");
                env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);
                Console.Out.WriteLine("Valor encontrado: " + env);
            }

            if (string.IsNullOrEmpty(env))
            {
                Console.Out.WriteLine("Obtendo valor da variável de ambiente ASPNETCORE_ENVIRONMENT para EnvironmentVariableTarget.Process");
                env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process);
                Console.Out.WriteLine("Valor encontrado: " + env);
            }

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

            Console.Title = "[API] - DocuSign - Assinatura Eletrônica de Documentos";
            LocalLog.WriteLogStart(configuration, "[API] DocuSign", "Assinatura Eletrônica de Documentos");

            host.Run();
        }
    }
}
