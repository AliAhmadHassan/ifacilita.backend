using Com.ByteAnalysis.IFacilita.CertiCadSp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Service
{
    public interface IRequisitionService
    {
        Requisition CreateOrUpdate(Requisition requisition);
        Requisition GetUnprocessed();
        List<Requisition> Get();
        Requisition Get(string id);
        void Remove(string id);
    }
}
