using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service
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

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddSingleton<Service.ICertificateService, Impl.CertificateService>();
            services.AddSingleton<Service.IOrderService, Impl.OrderService>();
            services.AddSingleton<Service.ExternalServices.SearchProtestApi.IClientApiSearchProtest, Service.ExternalServices.SearchProtestApi.Impl.ClientApiSearchProtest>();
            services.AddSingleton<Service.ExternalServices.DefectsDefinedApi.IDefectsDefinedClientApi, Service.ExternalServices.DefectsDefinedApi.Impl.DefectsDefinedClientApi>();
            services.AddSingleton<Service.ExternalServices.TaxDebtsApi.IClientTaxDebtsApi, Service.ExternalServices.TaxDebtsApi.Impl.ClientTaxDebtsApi>();
            services.AddSingleton<Service.ExternalServices.IptuDebtsApi.IClientIptuDebtsApi, Service.ExternalServices.IptuDebtsApi.Impl.ClientIptuDebtsApi>();
            services.AddSingleton<Service.ExternalServices.RealOnus.IClientRealOnusApi, Service.ExternalServices.RealOnus.Impl.ClientRealOnusApi>();
            services.AddSingleton<Service.ExternalServices.PropertyRegistrationDataApi.IClientPropertyRegistrationDataApi, Service.ExternalServices.PropertyRegistrationDataApi.Impl.ClientPropertyRegistrationDataApi>();
        }
    }
}
