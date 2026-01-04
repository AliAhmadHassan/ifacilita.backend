using Com.ByteAnalysis.IFacilita.RGI.Model;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.RGI.Service.Impl
{
    public class RequisitionService : IRequisitionService
    {
        Repository.IRequisitionRepository repository;

        public RequisitionService(Repository.IRequisitionRepository repository)
        {
            this.repository = repository;
        }

        public Requisition CreateOrUpdate(Requisition requisition) => this.repository.CreateOrUpdate(requisition);

        public IEnumerable<Requisition> Get() => repository.Get();

        public Requisition GetById(string Id)
        {
            return this.repository.GetById(Id);
        }

        public Requisition GetUnprocessed()
        {
            Model.Requisition requisition = this.repository.GetUnprocessed();

            if (requisition != null)
            {
                requisition.RpaStatus = Status.Processing;
                requisition.StatusModified = DateTime.Now;
            }

            return requisition;
        }
    }
}
