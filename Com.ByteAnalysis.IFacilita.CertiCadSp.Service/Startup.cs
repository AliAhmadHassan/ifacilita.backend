using Microsoft.Extensions.DependencyInjection;
using System;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Service
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
            services.AddSingleton<Service.IRequisitionService, Service.Impl.RequisitionService>();
        }
    }
}
