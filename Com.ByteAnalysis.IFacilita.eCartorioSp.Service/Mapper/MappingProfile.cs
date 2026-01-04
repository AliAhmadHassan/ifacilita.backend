using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Mapper
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            Mapper();
        }

        public void Mapper()
        {
            CreateMap<CertificatePropertyRegistrationDataEntity, ExternalServices.PropertyRegistrationDataApi.Dto.PropertyRegistrationDataRequestResponse>().ReverseMap();

            CreateMap<CertificateIptuDebtsEntity, ExternalServices.IptuDebtsApi.Dto.IptuDebtsRequestResponse>().ReverseMap();
            CreateMap<CertificateTaxDebtsEntity, ExternalServices.TaxDebtsApi.Dto.TaxDebtsRequestResponse>().ReverseMap();
            CreateMap<CertificateSearchProtestEntity, CertificateSearchProtestResponse>().ReverseMap();
            CreateMap<CertificateDefectsDefinedEntity, ExternalServices.DefectsDefinedApi.Dto.CertificateDefectsDefinedEntityRequestResponse>().ReverseMap();
            CreateMap<DataOrderEntity, ExternalServices.DefectsDefinedApi.Dto.DataOrderDto>().ReverseMap();

            CreateMap<CertificateRealOnusEntity, ExternalServices.RealOnus.Dto.RealOnusRequestResponse>()
                .ForMember(x => x.CallbackResponse, (y) => y.MapFrom(ya => ya.CallbackResponse))
                .ForMember(x => x.Expiration, (y) => y.MapFrom(ya => ya.Expiration))
                .ForMember(x => x.Id, (y) => y.MapFrom(ya => ya.Id))
                .ForMember(x => x.IdUser, (y) => y.MapFrom(ya => ya.IdUser))
                .ForMember(x => x.NumCartorio, (y) => y.MapFrom(ya => ya.Registry))
                .ForMember(x => x.NumMatricola, (y) => y.MapFrom(ya => ya.Registration))
                .ForMember(x => x.Protocolo, (y) => y.MapFrom(ya => ya.Protocol))
                .ForMember(x => x.Received, (y) => y.MapFrom(ya => ya.Received))
                .ForMember(x => x.Request, (y) => y.MapFrom(ya => ya.Request))
                .ForMember(x => x.Status, (y) => y.MapFrom(ya => ya.Status))
                .ForMember(x => x.StatusModified, (y) => y.MapFrom(ya => ya.StatusModified))
                .ForMember(x => x.UrlCallback, (y) => y.MapFrom(ya => ya.UrlCallback))
                .ForMember(x => x.UrlCertification, (y) => y.MapFrom(ya => ya.UrlCertification))
                .ForMember(x => x.s3patch, (y) => y.MapFrom(ya => ya.s3patch))
                .ReverseMap();
        }
    }


}
