using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service
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
            services.AddSingleton<Service.IMcriService, Impl.McriService>();
        }
    }
}
