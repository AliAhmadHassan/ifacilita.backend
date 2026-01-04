using Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi.Dto;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi
{
    public interface IClientTaxDebtsApi
    {
        TaxDebtsRequestResponse Get(string id);

        TaxDebtsRequestResponse Post(TaxDebtsRequestResponse body);
    }
}
