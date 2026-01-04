using Com.ByteAnalysis.IFacilita.Naturgy.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Repository.Impl
{

    public class RegisterClientRepository : IRegisterClientRepository
    {

        private readonly IMongoCollection<RegisterClient> _registerClient;

        public RegisterClientRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _registerClient = database.GetCollection<RegisterClient>("registerClient");
        }

        public RegisterClient Create(RegisterClient registerClient)
        {
            _registerClient.InsertOne(registerClient);
            return registerClient;
        }

        public RegisterClient CreateOrUpdate(RegisterClient registerClient)
        {
            if (String.IsNullOrEmpty(registerClient.Id))
            {
                registerClient.Request = DateTime.Now;
                _registerClient.InsertOne(registerClient);
            }
            else
            {
                _registerClient.ReplaceOne(requisitionIn => requisitionIn.Id.Equals(registerClient.Id), registerClient);
            }

            return registerClient;
        }

        public List<RegisterClient> Get() => _registerClient.Find<RegisterClient>(registerClient => true).ToList();


        public RegisterClient Get(string id) => _registerClient.Find<RegisterClient>(registerClient => registerClient.Id.Equals(id)).FirstOrDefault();


        public void Remove(RegisterClient registerClient) => _registerClient.DeleteOne(registerClient => registerClient.Id.Equals(registerClient.Id));


        public void Remove(string id) => _registerClient.DeleteOne(registerClient => registerClient.Id.Equals(id));



    }
}
