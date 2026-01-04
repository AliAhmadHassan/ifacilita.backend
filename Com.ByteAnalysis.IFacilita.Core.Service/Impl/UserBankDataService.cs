using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserBankDataService : IUserBankDataService
    {
        Repository.IUserBankDataRepository repository;

        public UserBankDataService(IUserBankDataRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<UserBankData> FindAll()
        {
            return repository.FindAll();
        }

        public UserBankData FindById(int id)
        {
            return repository.FindById(id);
        }

        public UserBankData Insert(UserBankData entity)
        {
            return repository.Insert(entity);
        }

        public UserBankData Update(UserBankData entity)
        {
            return repository.Update(entity);
        }
    }
}
