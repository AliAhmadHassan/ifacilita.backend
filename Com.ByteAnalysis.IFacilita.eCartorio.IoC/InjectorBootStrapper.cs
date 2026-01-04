using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Services;
using Com.ByteAnalysis.IFacilita.eCartorio.Cache;
using Com.ByteAnalysis.IFacilita.eCartorio.Data;
using Com.ByteAnalysis.IFacilita.eCartorio.Data.Impl;
using Com.ByteAnalysis.IFacilita.eCartorio.Data.Repositories;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Api;
using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Api.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

            //Cache
            services.AddSingleton<ICache<AuthenticationResponseeCartorio>, MemoryCache<AuthenticationResponseeCartorio>>();
            services.AddSingleton<ICache<IEnumerable<FuncaoListarFinalidadeResponse>>, MemoryCache<IEnumerable<FuncaoListarFinalidadeResponse>>>();
            services.AddSingleton<ICache<IEnumerable<FuncaoCartorioPorAtoResponse>>, MemoryCache<IEnumerable<FuncaoCartorioPorAtoResponse>>>();

            services.AddSingleton<ICache<IEnumerable<CategoriaListarResponse>>, MemoryCache<IEnumerable<CategoriaListarResponse>>>();
            services.AddSingleton<ICache<IEnumerable<CategoriaCertidoesPorCategoriaResponse>>, MemoryCache<IEnumerable<CategoriaCertidoesPorCategoriaResponse>>>();

            services.AddSingleton<ICache<IEnumerable<KitListarResponse>>, MemoryCache<IEnumerable<KitListarResponse>>>();
            services.AddSingleton<ICache<IEnumerable<KitCertidoesPorKitResponse>>, MemoryCache<IEnumerable<KitCertidoesPorKitResponse>>>();

            services.AddSingleton<ICache<IEnumerable<string>>, MemoryCache<IEnumerable<string>>>();

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IServiceBase<>), typeof(Application.Services.ServiceBase<>));

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogService, LogService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IKitService, KitService>();
            services.AddScoped<IActService, ActService>();
            services.AddScoped<IRequirementService, RequirementService>();

            services.AddScoped<IeCartorioApi, eCartorioApi>();
            services.AddScoped<IeCartorioClient, eCartorioClient>();
        }
    }
}
