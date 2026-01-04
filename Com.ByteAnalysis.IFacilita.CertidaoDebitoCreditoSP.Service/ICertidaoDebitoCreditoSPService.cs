using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Service
{
    public interface ICertidaoDebitoCreditoSPService
    {
        List<Model.CertidaoDebitoCreditoSP> Get();
        Model.CertidaoDebitoCreditoSP Get(string id);
        void Remove(string id);
        Model.CertidaoDebitoCreditoSP CreateOrUpdate(Model.CertidaoDebitoCreditoSP requisition);
        Model.CertidaoDebitoCreditoSP GetUnprocessed();
    }
}
