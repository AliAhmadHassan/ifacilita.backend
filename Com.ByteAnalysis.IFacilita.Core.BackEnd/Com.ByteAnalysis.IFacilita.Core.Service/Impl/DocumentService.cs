using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class DocumentService : IDocumentService
    {
        Repository.IDocumentRepository repository;

        public DocumentService(IDocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Document> FindAll()
        {
            return repository.FindAll();
        }

        public Document FindById(int id)
        {
            return repository.FindById(id);
        }

        public Document Insert(Document entity)
        {
            return repository.Insert(entity);
        }

        public Document Update(Document entity)
        {
            return repository.Update(entity);
        }
    }
}
