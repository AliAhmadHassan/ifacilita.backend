using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Service
{
    public interface ITransferIptuService
    {
        List<RequisitionModel> Get();

        RequisitionModel Get(string id);

        IEnumerable<RequisitionModel> GetPendings();

        RequisitionModel Create(RequisitionModel entry);

        void Update(string id, RequisitionModel entry);

        void Remove(RequisitionModel entry);

        void Remove(string id);
    }
}
