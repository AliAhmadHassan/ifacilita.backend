using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Repository
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<Model.IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<Model.MongoDatabaseSettings>>().Value);
            services.AddSingleton<Model.IApplicationSettings>(sp => sp.GetRequiredService<IOptions<Model.ApplicationSettings>>().Value);
            services.AddSingleton<Repository.IUserRepository, Repository.Impl.UserRepository>();
            services.AddSingleton<Repository.IToSendRepository, Repository.Impl.ToSendRepository>();
        }
    }
}
