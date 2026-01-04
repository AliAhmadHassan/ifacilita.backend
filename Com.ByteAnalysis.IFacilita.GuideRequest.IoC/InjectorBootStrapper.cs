using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Services;
using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Validators;
using Com.ByteAnalysis.IFacilita.GuideRequest.Data;
using Com.ByteAnalysis.IFacilita.GuideRequest.Data.Impl;
using Com.ByteAnalysis.IFacilita.GuideRequest.Data.Repositories;
using Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IGuideRequestService, GuideRequestService>();
            services.AddSingleton<IMongoDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
            services.AddSingleton<IGuideRequestRepository, GuideRequestRepository>();

            services.AddTransient<IValidator<GuideRequestInput>, GuideRequestValidation>();
        }   
    }
}
