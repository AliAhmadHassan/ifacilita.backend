using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.IptuDebtsApi
{
    public interface IClientIptuDebtsApi
    {
        IptuDebtsRequestResponse Get(string id);

        IptuDebtsRequestResponse Post(IptuDebtsRequestResponse body);
    }
}
