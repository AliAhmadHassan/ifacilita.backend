using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Repository
{
    public interface ICertidaoDebitoCreditoSPRepository
    {
        List<Model.CertidaoDebitoCreditoSP> Get();
        Model.CertidaoDebitoCreditoSP Get(string id);
        void Remove(string id);
        Model.CertidaoDebitoCreditoSP CreateOrUpdate(Model.CertidaoDebitoCreditoSP certidao);
        Model.CertidaoDebitoCreditoSP GetUnprocessed();
    }
}
