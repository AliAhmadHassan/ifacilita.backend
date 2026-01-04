using System;
using System.Collections.Generic;
using Com.ByteAnalysis.IFacilita.OnusReal.Model;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Repository
{
    public interface IRequisitionRepository
    {
        Requisition CreateOrUpdate(Requisition requisition);
        Requisition GetUnprocessed();
        List<Requisition> Get();
        Requisition Get(string id);
        Requisition Get(int registry, string registration);
        void Remove(string id);
    }
}
