using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserService : IUserService
    {
        Repository.IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<User> FindAll()
        {
            IEnumerable<User> users = repository.FindAll();

            foreach (var user in users)
            {
                user.Password = "";
            }

            return users;
        }

        public User FindById(int id)
        {
            User temp = repository.FindById(id);
            temp.Password = "";

            return temp;
        }

        public User FindBySocialLoginAuthorizationCode(string authorizationCode)
        {
            Entity.User temp = this.repository.FindBySocialLoginAuthorizationCode(authorizationCode);

            if (temp != null)
                temp.Password = "";

            return temp;
        }

        public User Insert(User entity)
        {
            User temp = repository.Insert(entity);
            temp.Password = "";

            return temp;
        }

        public User InsertWithSocialLogin(User user)
        {
            Entity.User temp = this.repository.FindByEMail(user.EMail);

            if (temp == null)
                temp = this.repository.Insert(user);

            if (temp != null)
                temp.Password = "";

            return temp;
        }

        public User Update(User entity)
        {
            User temp = repository.Update(entity);
            temp.Password = "";

            return temp;
        }
    }
}
