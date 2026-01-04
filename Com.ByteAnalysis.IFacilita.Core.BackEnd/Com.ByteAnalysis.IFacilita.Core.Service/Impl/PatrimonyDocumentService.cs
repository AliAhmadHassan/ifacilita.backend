using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyDocumentService : IPatrimonyDocumentService
    {
        Repository.IPatrimonyDocumentRepository repository;

        public PatrimonyDocumentService(IPatrimonyDocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<PatrimonyDocument> FindAll()
        {
            return repository.FindAll();
        }

        public PatrimonyDocument FindById(int id)
        {
            return repository.FindById(id);
        }

        public PatrimonyDocument Insert(PatrimonyDocument entity)
        {
            return repository.Insert(entity);
        }

        public PatrimonyDocument Update(PatrimonyDocument entity)
        {
            return repository.Update(entity);
        }
    }
}
