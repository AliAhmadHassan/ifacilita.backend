using Microsoft.Extensions.DependencyInjection;
using System;

namespace Com.ByteAnalysis.IFacilita.Chat.Service
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
            services.AddSingleton<Service.ITransactionService, Service.Impl.TransactionService>();
            services.AddSingleton<Service.IConnectionsService, Service.Impl.ConnectionsService>();
        }
    }
}
