namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Mapping
{
    public class DomainMappingProfile : AutoMapper.Profile
    {
        public DomainMappingProfile()
        {
            Mapped();
        }

        public void Mapped()
        {
            CreateMap<Models.GuideRequestInput, Domain.Entities.GuideRequest>().ReverseMap();
            CreateMap<Models.GenerationInput, Domain.Entities.Generation>().ReverseMap();
            CreateMap<Models.PurchaserTransmittedInput, Domain.Entities.PurchaserTransmitted>().ReverseMap();
            CreateMap<Models.PreProtocolInput, Domain.Entities.PreProtocol>().ReverseMap();
            CreateMap<Models.ProtocolInput, Domain.Entities.Protocol>().ReverseMap();
            CreateMap<Models.SimulateInput, Domain.Entities.Simulate>().ReverseMap();
            CreateMap<Models.GuideInput, Domain.Entities.Guide>().ReverseMap();
        }
    }
}
