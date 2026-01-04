using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserDocumentService : IUserDocumentService
    {
        Repository.IUserDocumentRepository repository;

        public UserDocumentService(IUserDocumentRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<UserDocument> FindAll()
        {
            return repository.FindAll();
        }

        public UserDocument FindById(int id)
        {
            return repository.FindById(id);
        }

        public UserDocument Insert(UserDocument entity)
        {
            return repository.Insert(entity);
        }

        public UserDocument Update(UserDocument entity)
        {
            return repository.Update(entity);
        }
    }
}
