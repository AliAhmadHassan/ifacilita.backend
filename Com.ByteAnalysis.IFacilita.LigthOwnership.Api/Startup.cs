using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prometheus;
using Prometheus.DotNetRuntime;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Api
{
    public class Startup
    {
        private LigthOwnership.Service.Startup serviceStartup;
        private readonly IWebHostEnvironment _env;

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

        public IDisposable Collector;

        public Startup(IWebHostEnvironment environment)
        {
            _env = environment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            serviceStartup = new LigthOwnership.Service.Startup();
            Collector = CreateCollector();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //var database = "ifacilitametrics";
            //var uri = new Uri("http://127.0.0.1:8086");

            //services.AddMetrics(opt =>
            //{
            //    opt.WithGlobalTags((gtag, info) =>
            //    {
            //        gtag.Add("app", info.EntryAssemblyName);
            //        gtag.Add("env", "stage");
            //    });
            //})
            //    .AddHealthChecks()
            //    .AddReporting(factory =>
            //    {
            //        factory.AddInfluxDb(
            //            new InfluxDBReporterSettings
            //            {
            //                InfluxDbSettings = new InfluxDBSettings(database, uri),
            //                ReportInterval = TimeSpan.FromSeconds(5)
            //            });
            //    })
            //    .AddMetricsMiddleware(opt => opt.IgnoredHttpStatusCodes = new[] { 404 });
            //services.AddMvc(opt => opt.AddMetricsResourceFilter());

            services.Configure<LigthOwnership.Model.MongoDatabaseSettings>(Configuration.GetSection(nameof(LigthOwnership.Model.MongoDatabaseSettings)));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                        options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGenNewtonsoftSupport();

            serviceStartup.ConfigureServices(services);

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Com.ByteAnalysis.IFacilita.LightOwnership",
                        Version = "v1",
                        Description = "API REST criada com o ASP.NET Core",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Francisco C B Silva"
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

            services.AddHealthChecks()
            .ForwardToPrometheus()
            .AddMongoDb(Configuration.GetConnectionString("BaseDados"), name: "Ligth");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpMetrics();
            app.UseMetricServer();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Com.ByteAnalysis.IFacilita.LigthOwnership");
            });
        }
    }
}
