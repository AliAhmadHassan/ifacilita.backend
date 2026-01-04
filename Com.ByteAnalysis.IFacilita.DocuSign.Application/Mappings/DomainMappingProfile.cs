namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Mapping
{
    public class DomainMappingProfile : AutoMapper.Profile
    {
        public DomainMappingProfile()
        {
            Mapped();
        }

        public void Mapped()
        {
            CreateMap<Models.UserInput, Domain.Entities.User>().ReverseMap();
            CreateMap<Models.EnvelopeDocuSignInput, Domain.Entities.EnvelopeDocuSign>().ReverseMap();
        }
    }
}
