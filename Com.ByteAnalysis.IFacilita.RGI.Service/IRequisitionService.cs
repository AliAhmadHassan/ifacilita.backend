using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.RGI.Service
{
    public interface IRequisitionService
    {
        Model.Requisition CreateOrUpdate(Model.Requisition requisition);
        
        Model.Requisition GetUnprocessed();

        IEnumerable<Model.Requisition> Get();

        Model.Requisition GetById(string id);
    }
}
