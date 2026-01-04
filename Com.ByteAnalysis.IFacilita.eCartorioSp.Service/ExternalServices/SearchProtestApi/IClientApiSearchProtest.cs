using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi
{
    public interface IClientApiSearchProtest
    {
        CertificateSearchProtestResponse Get(string id);

        CertificateSearchProtestResponse Post(CertificateSearchProtestResponse body);
    }
}
