using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service
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
