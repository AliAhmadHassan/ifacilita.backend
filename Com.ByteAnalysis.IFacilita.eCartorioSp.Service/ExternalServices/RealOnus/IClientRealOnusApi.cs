using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.RealOnus
{
    public interface IClientRealOnusApi
    {
        RealOnusRequestResponse Get(string id);

        RealOnusRequestResponse Post(RealOnusRequestResponse body);
    }
}
