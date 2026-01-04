using Com.ByteAnalysis.IFacilita.OnusReal.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Service
{
    public interface IRequisitionService
    {
        Requisition CreateOrUpdate(Requisition requisition);

        Requisition GetUnprocessed();

        List<Requisition> Get();

        Requisition Get(string id);

        Requisition Get(int registry, string registration);

        void Remove(string id);
    }
}
