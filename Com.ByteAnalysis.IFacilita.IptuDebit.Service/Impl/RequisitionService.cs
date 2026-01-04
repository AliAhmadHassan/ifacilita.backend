using Com.ByteAnalysis.IFacilita.IptuDebit.Model;
using System;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Service.Impl
{
    public class RequisitionService : IRequisitionService
    {
        Repository.IRequisitionRepository repository;

        public RequisitionService(Repository.IRequisitionRepository repository)
        {
            this.repository = repository;
        }

        public Requisition CreateOrUpdate(Requisition requisition) => this.repository.CreateOrUpdate(requisition);

        public Requisition Get(string id) => this.repository.Get(id);

        public Requisition GetUnprocessed() => repository.GetUnprocessed();
    }
}
