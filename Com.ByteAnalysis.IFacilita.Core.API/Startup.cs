using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.EmailService;
using Com.ByteAnalysis.IFacilita.Core.Api.HealthCheck;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Prometheus;
using Prometheus.DotNetRuntime;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;


namespace Com.ByteAnalysis.IFacilita.Core.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IDisposable Collector;

        private Service.Startup serviceStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Collector = CreateCollector();

            serviceStartup = new Service.Startup();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            serviceStartup.ConfigureServices(services);
            services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.Configure<SmtpSettings>(Configuration.GetSection(nameof(SmtpSettings)));
            services.AddSingleton<ISmtpSettings>(Span => Span.GetRequiredService<IOptions<SmtpSettings>>().Value);

            services.Configure<PathSettings>(Configuration.GetSection(nameof(PathSettings)));
            services.AddSingleton<IPathSettings>(Span => Span.GetRequiredService<IOptions<PathSettings>>().Value);

            services.Configure<ApplicationSettings >(Configuration.GetSection(nameof(ApplicationSettings )));
            services.AddSingleton<IApplicationSettings>(Span => Span.GetRequiredService<IOptions<ApplicationSettings >>().Value);

            services.AddCors();

            //services.AddMvc(options => options.Filters.Add(new MiddlewareFilterAttribute(typeof(MyFilter))))

            services.AddCors(options =>
            {
                options.AddPolicy("CorsApi",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Configurando o serviço de documentação do Swagger
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Com.ByteAnalysis.IFacilita",
                        Version = "v1",
                        Description = "API REST criada com o ASP.NET Core",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Ali Ahmad Hassan"
                        }
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });

            //API de Envio de Emails
            services.AddTransient<IClientEmailService, ClientEmailService>();

            //Envio de Emails pelo MailKit
            try
            {
                var configSMTP = Configuration.GetSection("EmailConfiguration");
                var confs = configSMTP.Get<EmailConfiguration>();

                services.AddSingleton<IEmailConfiguration>(confs);
                services.AddTransient<IEmailService, EmailService>();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine($"[ERROR] - [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {ex.Message}");
            }

            services.AddHealthChecks()
                .AddCheck<ECartorioRjHealthCheck>("eCartorioRJ")
                .AddCheck<ECartorioSpHealthCheck>("eCartorioSP")
                .ForwardToPrometheus()
                .AddSqlServer(Configuration.GetConnectionString("BaseDados"), name: "Base de dados")
                .AddMongoDb(Configuration.GetConnectionString("BaseLog"), name: "Base de logs");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpMetrics();
            app.UseMetricServer();

            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseHealthChecks("/health",
             new HealthCheckOptions()
             {
                 ResponseWriter = async (context, report) =>
                 {
                     var result = JsonConvert.SerializeObject(
                         new
                         {
                             statusApplication = report.Status.ToString(),
                             healthChecks = report.Entries.Select(e => new
                             {
                                 check = e.Key,
                                 ErrorMessage = e.Value.Exception?.Message,
                                 status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                             })
                         });
                     context.Response.ContentType = MediaTypeNames.Application.Json;
                     await context.Response.WriteAsync(result);
                 }
             });

            app.UseRouting();
            app.UseCors("CorsApi");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Com.ByteAnalysis.IFacilita");
            });
        }

        public static IDisposable CreateCollector()
        {
            var builder = DotNetRuntimeStatsBuilder.Default();
            builder = DotNetRuntimeStatsBuilder.Customize()
                .WithContentionStats(CaptureLevel.Informational)
                .WithGcStats(CaptureLevel.Verbose)
                .WithThreadPoolStats(CaptureLevel.Informational)
                .WithExceptionStats(CaptureLevel.Errors)
                .WithJitStats();
            return builder.StartCollecting();

        }
    }
}
