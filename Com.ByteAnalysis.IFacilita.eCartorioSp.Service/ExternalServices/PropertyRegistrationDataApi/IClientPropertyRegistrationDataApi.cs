using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi
{
    public interface IClientPropertyRegistrationDataApi
    {
        PropertyRegistrationDataRequestResponse Get(string id);

        PropertyRegistrationDataRequestResponse Post(PropertyRegistrationDataRequestResponse body);
    }
}
