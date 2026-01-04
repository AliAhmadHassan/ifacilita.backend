using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserProfileService : IUserProfileService
    {
        Repository.IUserProfileRepository repository;

        public UserProfileService(IUserProfileRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<UserProfile> FindAll()
        {
            return repository.FindAll();
        }

        public UserProfile FindById(int id)
        {
            return repository.FindById(id);
        }

        public UserProfile Insert(UserProfile entity)
        {
            return repository.Insert(entity);
        }

        public UserProfile Update(UserProfile entity)
        {
            return repository.Update(entity);
        }
    }
}
