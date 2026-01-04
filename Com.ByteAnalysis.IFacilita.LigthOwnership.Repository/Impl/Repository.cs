using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Repository.Impl
{
    public class Repository : IRepository
    {
        private readonly IMongoCollection<OwnershipModel> _collection;

        public Repository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<OwnershipModel>("Ownership");
        }

        public OwnershipModel Create(OwnershipModel owner)
        {
            _collection.InsertOne(owner);
            return owner;
        }

        public List<OwnershipModel> Get() => _collection.Find<OwnershipModel>(owner => true).ToList();

        public OwnershipModel Get(string id) => _collection.Find<OwnershipModel>(owner => owner.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<OwnershipModel> GetPendings() => _collection.Find<OwnershipModel>(t => t.Pending.Equals(1)).ToList();

        public void Remove(OwnershipModel owner) => _collection.DeleteOne(owner => owner.Id.Equals(owner.Id));

        public void Remove(string id) => _collection.DeleteOne(owner => owner.Id.Equals(id));

        public void Update(string id, OwnershipModel owner) => _collection.ReplaceOne(owner => owner.Id == id, owner);


    }
}
