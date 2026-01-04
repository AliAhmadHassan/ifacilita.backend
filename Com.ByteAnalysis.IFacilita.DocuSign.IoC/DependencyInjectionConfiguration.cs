using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.DocuSign.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            InjectorBootStrapper.RegisterServices(services);
        }
    }
}
