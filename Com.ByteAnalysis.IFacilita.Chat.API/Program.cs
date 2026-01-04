using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.Chat.API
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            var env = GetEnvironment();
            Console.Out.WriteLine($"Passando para o próximo passo.");

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.{env}.json", true)
             .AddJsonFile($"hosting.{env}.json", true,true);

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
                .UseKestrel(op =>
                {
                    if (env.Equals("Production"))
                    {
                        op.Listen(IPAddress.Parse("10.0.0.4"), 5401);
                        op.Listen(IPAddress.Parse("10.0.0.4"), 5400, opt =>
                        {
                            opt.UseHttps("ifacilita-ifacilita4c41ddd4-3eef-4344-a7ae-270c5d513550-20201117.pfx", "");
                        });
                    }
                    else if (env.Equals("Test")) { }
                    else { }
                })
                .UseStartup<Startup>()
                .Build();
            
            Console.Title = "[API] Chat - Chat iFacilita";
            LocalLog.WriteLogStart(configuration, "[API] Chat", "Chat iFacilita");

            host.Run();
        }

        public static string GetEnvironment()
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

            return env;
        }
    }
}

