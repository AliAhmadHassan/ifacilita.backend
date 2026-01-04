using Com.ByteAnalysis.IFacilita.PushNotification.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Model.User> _users;

        public UserRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<Model.User>("users");
        }

        public User CreateOrUpdate(User user)
        {
            if (user.Id != null && user.Id != "")
                this._users.ReplaceOne(u => u.Id == user.Id, user);
            else
                this._users.InsertOne(user);

            return user;
        }

        public Model.User Get(string id) => _users.Find<Model.User>(user => user.Id.Equals(id)).FirstOrDefault();
    }
}
