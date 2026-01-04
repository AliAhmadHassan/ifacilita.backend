using Com.ByteAnalysis.IFacilita.OnusReal.Model;
using Com.ByteAnalysis.IFacilita.OnusReal.Repository;
using System;
using System.Collections.Generic;
using System.Text;


namespace Com.ByteAnalysis.IFacilita.OnusReal.Service.Impl
{
    public class RequisitionService : IRequisitionService
    {
        IRequisitionRepository repository;

        public RequisitionService(IRequisitionRepository repository)
        {
            this.repository = repository;   
        }

        public Requisition CreateOrUpdate(Requisition requisition) => this.repository.CreateOrUpdate(requisition);

        public Requisition GetUnprocessed()
        {
            Requisition requisition = this.repository.GetUnprocessed();
            return requisition;
        }

        public List<Requisition> Get() => repository.Get();

        public Requisition Get(string id) => repository.Get(id);

        public void Remove(string id) => repository.Remove(id);

        public Requisition Get(int registry, string registration) => repository.Get(registry, registration);
    }
}
