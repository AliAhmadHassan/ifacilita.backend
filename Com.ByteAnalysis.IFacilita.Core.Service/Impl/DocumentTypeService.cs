using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class DocumentTypeService : IDocumentTypeService
    {
        Repository.IDocumentTypeRepository repository;

        public DocumentTypeService(IDocumentTypeRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<DocumentType> FindAll()
        {
            return repository.FindAll();
        }

        public DocumentType FindById(int id)
        {
            return repository.FindById(id);
        }

        public DocumentType Insert(DocumentType entity)
        {
            return repository.Insert(entity);
        }

        public DocumentType Update(DocumentType entity)
        {
            return repository.Update(entity);
        }
    }
}
