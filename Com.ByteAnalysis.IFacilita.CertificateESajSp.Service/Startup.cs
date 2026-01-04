using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Service
{
    public class Startup
    {
        Repository.Startup repositoryStartup;
        public Startup()
        {
            repositoryStartup = new Repository.Startup();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            repositoryStartup.ConfigureServices(services);
            services.AddSingleton<Service.ICertificateService, Impl.CertificateService>();
        }
    }
}
