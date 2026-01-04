using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class SendInviteTextTypeService : ISendInviteTextTypeService
    {
        Repository.ISendInviteTextTypeRepository repository;

        public SendInviteTextTypeService(ISendInviteTextTypeRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<SendInviteTextType> FindAll()
        {
            return repository.FindAll();
        }

        public SendInviteTextType FindById(int id)
        {
            return repository.FindById(id);
        }

        public SendInviteTextType Insert(SendInviteTextType entity)
        {
            return repository.Insert(entity);
        }

        public SendInviteTextType Update(SendInviteTextType entity)
        {
            return repository.Update(entity);
        }
    }
}
