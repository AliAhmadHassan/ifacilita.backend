using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class SendInviteService : ISendInviteService
    {
        Repository.ISendInviteRepository repository;

        public SendInviteService(ISendInviteRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<SendInvite> FindAll()
        {
            return repository.FindAll();
        }

        public SendInvite FindById(int id)
        {
            return repository.FindById(id);
        }

        public SendInvite Insert(SendInvite entity)
        {
            return repository.Insert(entity);
        }

        public SendInvite Update(SendInvite entity)
        {
            return repository.Update(entity);
        }
    }
}
