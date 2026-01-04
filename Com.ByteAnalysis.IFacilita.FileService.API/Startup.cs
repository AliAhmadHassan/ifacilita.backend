using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Prometheus;
using Prometheus.DotNetRuntime;
using System;
using System.Linq;
using System.Net.Mime;

namespace Com.ByteAnalysis.IFacilita.FileService.API
{
    public class Startup
    {
        public IDisposable Collector;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Collector = CreateCollector();
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<AmazonAWSS3Settings>(Configuration.GetSection(nameof(AmazonAWSS3Settings)));
            services.AddSingleton<IAmazonAWSS3Settings>(sp => sp.GetRequiredService<IOptions<AmazonAWSS3Settings>>().Value);

            services
                .AddHealthChecks()
                .ForwardToPrometheus();
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
        }
    }
}
