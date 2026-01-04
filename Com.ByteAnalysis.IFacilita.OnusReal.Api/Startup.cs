using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using System.IO;
using System;
using Prometheus;
using Prometheus.DotNetRuntime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Com.ByteAnalysis.IFacilita.OnusReal.Api.HealthCheck;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Api
{
    public class Startup
    {
        private Service.Startup serviceStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            serviceStartup = new Service.Startup();
            Collector = CreateCollector();
        }

        public IDisposable Collector;
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
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Model.MongoDatabaseSettings>(Configuration.GetSection(nameof(Model.MongoDatabaseSettings)));
            serviceStartup.ConfigureServices(services);

            services.AddControllers()
              .AddNewtonsoftJson(options =>
                      options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "RealOnus - Ônus Reais",
                        Version = "v1",
                        Description = "RPA para a extração da certidão de Onus Reais do imóvel - [https://www.registradores.org.br/eProtocolo/DefaultAC.aspx]",
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
            services.AddHealthChecks()
                .AddCheck<SiteHealthCheck>("Site-OnusReal")
                .ForwardToPrometheus()
                .AddMongoDb(Configuration.GetConnectionString("BaseDados"), name: "BaseDados-OnusReal");
        }

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
                    "Com.ByteAnalysis.IFacilita.OnusReal");
            });
        }
    }
}
