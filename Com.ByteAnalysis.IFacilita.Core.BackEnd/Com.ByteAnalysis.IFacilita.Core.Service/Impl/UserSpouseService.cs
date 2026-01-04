using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserSpouseService : IUserSpouseService
    {
        Repository.IUserSpouseRepository repository;

        public UserSpouseService(IUserSpouseRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<UserSpouse> FindAll()
        {
            return repository.FindAll();
        }

        public UserSpouse FindById(int id)
        {
            return repository.FindById(id);
        }

        public UserSpouse Insert(UserSpouse entity)
        {
            return repository.Insert(entity);
        }

        public UserSpouse Update(UserSpouse entity)
        {
            return repository.Update(entity);
        }
    }
}
