using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using Com.ByteAnalysis.IFacilita.TransfIptuSp.Repository.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Repository
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<ITransferIptuRepository, TransferIptRepository>();
        }
    }
}
