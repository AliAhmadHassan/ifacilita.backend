using Com.ByteAnalysis.IFacilita.Mcri.Model;
using Com.ByteAnalysis.IFacilita.Mcri.Repository;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service.Impl
{
    public class McriService : IMcriService
    {
        private readonly IMcriRepository _repository;

        public McriService(IMcriRepository repository)
        {
            _repository = repository;
        }

        public CriModel Create(CriModel cri) => _repository.Create(cri);

        public List<CriModel> Get() => _repository.Get();

        public CriModel Get(string id) => _repository.Get(id);

        public IEnumerable<CriModel> GetPendings() => _repository.GetPendings();

        public void Remove(CriModel cri) => _repository.Remove(cri);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, CriModel cri) => _repository.Update(id, cri);
    }
}
