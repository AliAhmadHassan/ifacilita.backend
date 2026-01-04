using System;
using System.Collections.Generic;
using Com.ByteAnalysis.IFacilita.CertiCadSp.Model;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Repository
{
    public interface IRequisitionRepository
    {
        Requisition CreateOrUpdate(Requisition requisition);
        Requisition GetUnprocessed();
        List<Requisition> Get();
        Requisition Get(string id);
        void Remove(string id);
    }
}
