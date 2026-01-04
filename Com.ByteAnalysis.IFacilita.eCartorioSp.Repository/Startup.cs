using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Repository
{
    public class Startup
    {
        public Startup()
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<Repository.ICertificateRepository, Impl.CertificateRepository>();
            services.AddSingleton<Repository.IOrderRepository, Impl.OrderRepository>();
        }
    }
}
