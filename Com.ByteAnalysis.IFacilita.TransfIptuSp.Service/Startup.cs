using Com.ByteAnalysis.IFacilita.TransfIptuSp.Service.Imp;
using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Service
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
            services.AddSingleton<ITransferIptuService, TransferIptuService>();
        }
    }
}
