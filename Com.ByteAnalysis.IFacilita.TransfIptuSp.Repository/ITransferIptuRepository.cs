using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Repository
{
    public interface ITransferIptuRepository
    {
        List<RequisitionModel> Get();

        RequisitionModel Get(string id);

        IEnumerable<RequisitionModel> GetPendings();

        RequisitionModel Create(RequisitionModel entity);

        void Update(string id, RequisitionModel entity);

        void Remove(RequisitionModel entity);

        void Remove(string id);
    }
}
