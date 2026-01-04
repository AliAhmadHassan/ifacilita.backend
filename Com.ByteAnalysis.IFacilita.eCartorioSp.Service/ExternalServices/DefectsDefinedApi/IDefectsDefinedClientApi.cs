using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi
{
    public interface IDefectsDefinedClientApi
    {
        CertificateDefectsDefinedEntityRequestResponse Get(string id);

        CertificateDefectsDefinedEntityRequestResponse Post(CertificateDefectsDefinedEntityRequestResponse body);
    }
}
