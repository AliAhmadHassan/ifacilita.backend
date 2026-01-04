using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Services;
using Com.ByteAnalysis.IFacilita.DocuSign.Client;
using Com.ByteAnalysis.IFacilita.DocuSign.Client.Api;
using Com.ByteAnalysis.IFacilita.DocuSign.Data;
using Com.ByteAnalysis.IFacilita.DocuSign.Data.Impl;
using Com.ByteAnalysis.IFacilita.DocuSign.Data.Repositories;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.DocuSign.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserDocuSignRepository, UserDocuSignRepository>();
            services.AddScoped<IEnvelopeDocuSignRepository, EnvelopeDocuSignRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            services.AddScoped(typeof(IServiceBase<>), typeof(Application.Services.ServiceBase<>));
            services.AddScoped<IUserDocuSignService, UserDocuSignService>();
            services.AddScoped<IEnvelopeDocuSignService, EnvelopeDocuSignService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped(typeof(IDocuSignApi<>), typeof(DocuSignApi<>));
            services.AddScoped<IDocuSignClient, DocuSignClient>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
