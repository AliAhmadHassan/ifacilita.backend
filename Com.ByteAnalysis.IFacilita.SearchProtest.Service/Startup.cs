using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Service
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
            services.AddSingleton<Service.ISearchProtestService, Impl.SearchProtestService>();
        }
    }
}
