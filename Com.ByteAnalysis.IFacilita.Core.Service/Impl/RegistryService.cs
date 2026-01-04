using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class RegistryService : IRegistryService
    {
        IRegistryRepository _repository;

        public RegistryService(IRegistryRepository repository) => _repository = repository;

        public void Delete(int id) => _repository.Delete(id);

        public IEnumerable<Registry> FindAll() => _repository.FindAll();

        public IEnumerable<Registry> FindByCity(string city) => _repository.FindByCity(city);

        public Registry FindById(int id) => _repository.FindById(id);

        public IEnumerable<Registry> FindCloser(int idtransaction) => _repository.FindCloser(idtransaction);

        public Registry Insert(Registry entity) => _repository.Insert(entity);

        public Registry Update(Registry entity) => _repository.Update(entity);
    }
}
