using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.RGI.Repository
{
    public interface IRequisitionRepository
    {
        Model.Requisition CreateOrUpdate(Model.Requisition requisition);

        Model.Requisition GetUnprocessed();

        IEnumerable<Model.Requisition> Get();

        Model.Requisition GetById(string id);
    }
}
