using Com.ByteAnalysis.IFacilita.PushNotification.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Service.Impl
{
    public class UserService : IUserService
    {
        Repository.IUserRepository repository;
        public UserService(Repository.IUserRepository repository)
        {
            this.repository = repository;
        }

        public User AddToken(string id, string token)
        {
            Model.User user = Get(id);

            if (!user.Tokens.Contains(token))
                user.Tokens.Add(token);

            user = CreateOrUpdate(user);

            return user;
        }

        public User CreateOrUpdate(User user)
        {
            return this.repository.CreateOrUpdate(user);
        }

        public User Get(string id)
        {
            return this.repository.Get(id);
        }
    }
}
