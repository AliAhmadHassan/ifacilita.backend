using Com.ByteAnalysis.IFacilita.Chat.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Com.ByteAnalysis.IFacilita.Chat.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<Repository.ITransactionRepository, Repository.Impl.TransactionRepository>();
            services.AddSingleton<Repository.IConnectionsRepository, Repository.Impl.ConnectionsRepository>();
        }
    }
}
