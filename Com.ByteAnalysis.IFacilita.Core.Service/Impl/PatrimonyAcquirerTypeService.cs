using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyAcquirerTypeService : IPatrimonyAcquirerTypeService
    {
        Repository.IPatrimonyAcquirerTypeRepository repository;

        public PatrimonyAcquirerTypeService(IPatrimonyAcquirerTypeRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PatrimonyAcquirerType> FindAll()
        {
            return repository.FindAll();
        }

        public PatrimonyAcquirerType FindById(int id)
        {
            return repository.FindById(id);
        }

        public PatrimonyAcquirerType Insert(PatrimonyAcquirerType entity)
        {
            return repository.Insert(entity);
        }

        public PatrimonyAcquirerType Update(PatrimonyAcquirerType entity)
        {
            return repository.Update(entity);
        }
    }
}
