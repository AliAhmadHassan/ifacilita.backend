using Com.ByteAnalysis.IFacilita.LigthOwnership.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<IRepository, Impl.Repository>();
        }
    }
}
