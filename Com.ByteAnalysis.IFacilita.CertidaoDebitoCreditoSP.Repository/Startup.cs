using Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<Repository.ICertidaoDebitoCreditoSPRepository, Repository.Impl.CertidaoDebitoCreditoSP>();
        }
    }
}
