using System;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Service
{
    public interface IRequisitionService
    {
        Model.Requisition CreateOrUpdate(Model.Requisition requisition);
        Model.Requisition GetUnprocessed();
        Model.Requisition Get(string id);
    }
}
