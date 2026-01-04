using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Repository;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Service.Impl
{
    public class Service : IService
    {
        private readonly IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public OwnershipModel Create(OwnershipModel owner) => _repository.Create(owner);

        public List<OwnershipModel> Get() => _repository.Get();

        public OwnershipModel Get(string id) => _repository.Get(id);

        public IEnumerable<OwnershipModel> GetPendings() => _repository.GetPendings();

        public void Remove(OwnershipModel owner) => _repository.Remove(owner);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, OwnershipModel owner) => _repository.Update(id, owner);
    }
}
