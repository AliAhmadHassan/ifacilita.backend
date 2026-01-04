using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using Com.ByteAnalysis.IFacilita.ITBISP.Repository;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service.Impl
{
    public class RequisitionService : IRequisitionService
    {
        private readonly IRequisitionRepository _repository;

        public RequisitionService(IRequisitionRepository repository)
        {
            _repository = repository;
        }

        public RequisitionItbiModel CreateOrUpdate(RequisitionItbiModel requisition) => this._repository.CreateOrUpdate(requisition);

        public RequisitionItbiModel GetUnprocessed() => this._repository.GetUnprocessed();

        public List<RequisitionItbiModel> Get() => _repository.Get();

        public RequisitionItbiModel Get(string id) => _repository.Get(id);

        public void Remove(string id) => _repository.Remove(id);
    }
}
