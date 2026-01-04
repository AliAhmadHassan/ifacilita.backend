using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class RegistryService : IRegistryService
    {
        Repository.IRegistryRepository repository;

        public RegistryService(IRegistryRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Registry> FindAll()
        {
            return repository.FindAll();
        }

        public Registry FindById(int id)
        {
            return repository.FindById(id);
        }

        public Registry Insert(Registry entity)
        {
            return repository.Insert(entity);
        }

        public Registry Update(Registry entity)
        {
            return repository.Update(entity);
        }
    }
}
