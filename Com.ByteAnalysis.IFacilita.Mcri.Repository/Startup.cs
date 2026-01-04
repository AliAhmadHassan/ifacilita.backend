using Com.ByteAnalysis.IFacilita.Mcri.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.Mcri.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<Repository.IMcriRepository, Impl.McriRepository>();
        }
    }
}
