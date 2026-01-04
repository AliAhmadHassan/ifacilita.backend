using Com.ByteAnalysis.IFacilita.RGI.Api.HealthCheck;
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

namespace Com.ByteAnalysis.IFacilita.RGI.Api
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<Com.ByteAnalysis.IFacilita.RGI.Model.MongoDatabaseSettings>(Configuration.GetSection(nameof(Com.ByteAnalysis.IFacilita.RGI.Model.MongoDatabaseSettings)));
            services.AddControllers();
            serviceStartup.ConfigureServices(services);

            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsApi",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "RPA - Registro de Imóveis",
                        Version = "v1",
                        Description = "RPA para atuar com o Registro de Imóveis - [https://www.registradores.org.br/]",
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
                .AddCheck<SiteHealthCheck>("Site-Rgi_Rj")
            .ForwardToPrometheus()
            .AddMongoDb(Configuration.GetConnectionString("BaseDados"), name: "BaseDados-Rgi_Rj")
            .AddMongoDb(Configuration.GetConnectionString("BaseLogs"), name: "BaseLogs-Rgi_Rj");
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

            app.UseCors("CorsApi");

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
                    "Com.ByteAnalysis.IFacilita.RGI.Api");
            });
        }
    }
}
