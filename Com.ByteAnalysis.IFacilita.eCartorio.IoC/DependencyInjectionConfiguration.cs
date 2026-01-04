using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.eCartorio.IoC
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            InjectorBootStrapper.RegisterServices(services);
        }
    }
}
