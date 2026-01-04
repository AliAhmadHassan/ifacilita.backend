using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.Core.Api
{
    public class Program
    {
        private static IConfiguration configuration;

        public static void Main(string[] args)
        {
            Console.Out.WriteLine("Iniciando o sistema iFacilita Core");
            try
            {
                var env = GetEnvironment();
                Log.Information($"Passando para o próximo passo.");

                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.{env}.json", true)
                 .AddJsonFile($"hosting.{env}.json", true, true);

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
               .ConfigureAppConfiguration((host, config) =>
                {
                    var settings = config.Build();
                    Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.MongoDB(settings.GetConnectionString("BaseLog"), collectionName: "Core.API")
                    .WriteTo.File("iFacilita-Core.log", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console()
                    .CreateLogger();
                })
               //.UseKestrel()
               //.UseKestrel((ctx, opt) =>
               // {
               //     opt.Configure(ctx.Configuration.GetSection("Kestrel"))
               //     .Endpoint("HTTPS", lst =>
               //     {
               //         lst.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
               //     });
               // })
               .UseKestrel(op =>
               {
                   if (env.Equals("Production"))
                   {
                       op.Listen(IPAddress.Parse("10.0.0.4"), 5001);
                       op.Listen(IPAddress.Parse("10.0.0.4"), 5000, opt =>
                       {
                           opt.UseHttps("ifacilita-ifacilita4c41ddd4-3eef-4344-a7ae-270c5d513550-20201117.pfx", "");
                       });
                   }
                   else if (env.Equals("Test"))
                   {
                       //op.Listen(IPAddress.Parse("127.0.0.1"), 80);
                       //op.Listen(IPAddress.Parse("127.0.0.1"), 443, opt =>
                       //{
                       //    opt.UseHttps("ifacilita-ifacilita4c41ddd4-3eef-4344-a7ae-270c5d513550-20201117.pfx", "");
                       //});
                   }
                   else
                   {
                       //op.Listen(IPAddress.Parse("127.0.0.1"), 5000);
                   }
               })
               .UseStartup<Startup>()
               .Build();

                Log.Information("Iniciando Web Host iFacilita - Core");
                Log.Information($"Ambiente de execução: {env}");

                host.Run();

            }
            catch (Exception ex)
            {
                Serilog.Log.Fatal(ex, "Host encerrado inesperadamente");
            }
            finally
            {
                Serilog.Log.CloseAndFlush();
            }
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

        #region Removido
        //public static void Main(string[] args)
        //{
        //    var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory());


        //    if (File.Exists("appsettings.prod.json"))
        //        builder.AddJsonFile("appsettings.prod.json");
        //    else
        //        builder.AddJsonFile("appsettings.json");

        //    configuration = builder.Build();



        //    Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        //    .Enrich.FromLogContext()
        //    .WriteTo.Console()
        //    .WriteTo.MongoDB(configuration.GetValue<string>("DatabaseSettings:BaseLog"), collectionName: "Core.API")
        //    .CreateLogger();

        //    try
        //    {


        //        var host = new WebHostBuilder()
        //            .UseConfiguration(configuration)
        //            .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))

        //            .ConfigureLogging(b =>
        //            {
        //                b.SetMinimumLevel(LogLevel.Error);
        //                b.AddConfiguration(configuration);
        //                b.AddConsole();
        //            })
        //            .UseKestrel();

        //        if (File.Exists("appsettings.prod.json"))
        //            host.UseKestrel(
        //                op =>
        //                {
        //                    op.Listen(IPAddress.Parse("10.0.0.4"), 5000, opt =>
        //                    {
        //                        opt.UseHttps("ifacilita-ifacilita4c41ddd4-3eef-4344-a7ae-270c5d513550-20201117.pfx", "");
        //                    });
        //                });
        //        else
        //            host.UseUrls(new string[] { "http://*:5000" });

        //        Log.Information("Starting web host");

        //        host.UseStartup<Startup>()
        //        .UseSerilog() // <-- Add this line
        //        .Build()
        //        .Run();

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Fatal(ex, "Host terminated unexpectedly");
        //    }
        //    finally
        //    {
        //        Log.CloseAndFlush();
        //    }
        //}

        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });*/
        #endregion
    }
}
