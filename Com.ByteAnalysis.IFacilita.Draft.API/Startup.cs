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
using Prometheus;
using Prometheus.DotNetRuntime;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace Com.ByteAnalysis.IFacilita.Draft.API
{
    public class Startup
    {
        private Service.Startup serviceStartup;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            serviceStartup = new Service.Startup();
            Collector = CreateCollector();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Com.ByteAnalysis.IFacilita.Draft.Model.MongoDatabaseSettings>(Configuration.GetSection(nameof(Com.ByteAnalysis.IFacilita.Draft.Model.MongoDatabaseSettings)));

            services.AddControllers();
            serviceStartup.ConfigureServices(services);

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Com.ByteAnalysis.IFacilita.Draft",
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

            services.AddHealthChecks()
            .ForwardToPrometheus()
            .AddMongoDb(Configuration.GetConnectionString("BaseDados"), name: "Draft");
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

            app.UseHttpsRedirection();

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
                    "Com.ByteAnalysis.IFacilita.Draft");
            });
        }
    }
}
