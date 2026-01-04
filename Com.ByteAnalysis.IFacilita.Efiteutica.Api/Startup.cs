using Com.ByteAnalysis.IFacilita.Efiteutica.Api.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
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
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Api
{
    public class Startup
    {
        private Service.Startup serviceStartup;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Collector = CreateCollector();

            serviceStartup = new Service.Startup();
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

        public IDisposable Collector;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Model.MongoDatabaseSettings>(Configuration.GetSection(nameof(Model.MongoDatabaseSettings)));
            serviceStartup.ConfigureServices(services);

            services.AddResponseCompression(
               options =>
               {
                   options.Providers.Add<BrotliCompressionProvider>();
                   options.Providers.Add<GzipCompressionProvider>();
               });

            services.Configure<BrotliCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

            services.Configure<GzipCompressionProviderOptions>(
                options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

            services.AddControllers()
               .AddNewtonsoftJson(options =>
                       options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Com.ByteAnalysis.IFacilita.Efiteutica.Api",
                        Version = "v1",
                        Description = "API REST criada com o ASP.NET Core 3.1",
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

            services
                .AddHealthChecks()
                .AddCheck<EfiteuticaSiteHealthCheck>("Site-Efiteutica")
                .AddMongoDb(Configuration.GetConnectionString("BaseDados"), name: "BaseDados-Efiteutica")
                .ForwardToPrometheus();
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
                    "Com.ByteAnalysis.IFacilita.Efiteutica.Api");
            });
        }
    }
}
