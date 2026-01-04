using System;
using System.Collections.Generic;
using System.Text;
using Com.ByteAnalysis.IFacilita.OnusReal.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<Repository.IRequisitionRepository, Repository.Impl.RequisitionRepository>();
        }

    }
}
