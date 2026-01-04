using Com.ByteAnalysis.IFacilita.CertiCadSp.Model;
using Com.ByteAnalysis.IFacilita.CertiCadSp.Repository;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Service.Impl
{
    public class RequisitionService : IRequisitionService
    {
        IRequisitionRepository repository;

        public RequisitionService(IRequisitionRepository repository)
        {
            this.repository = repository;
        }

        public Requisition CreateOrUpdate(Requisition requisition) => this.repository.CreateOrUpdate(requisition);

        public Requisition GetUnprocessed()=> repository.GetUnprocessed();

        public List<Requisition> Get() => repository.Get();

        public Requisition Get(string id) => repository.Get(id);

        public void Remove(string id) => repository.Remove(id);
    }
}
