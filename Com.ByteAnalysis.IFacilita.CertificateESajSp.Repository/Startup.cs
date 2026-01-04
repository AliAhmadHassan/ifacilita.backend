using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using Com.ByteAnalysis.IFacilita.CertificateESajSp.Repository.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Repository
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<ICertificateRepository, CertificateRepository>();
        }
    }
}
