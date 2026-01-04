using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service
{
    public interface IRequisitionService
    {
        RequisitionItbiModel CreateOrUpdate(RequisitionItbiModel requisition);

        RequisitionItbiModel GetUnprocessed();

        List<RequisitionItbiModel> Get();

        RequisitionItbiModel Get(string id);

        void Remove(string id);
    }
}
