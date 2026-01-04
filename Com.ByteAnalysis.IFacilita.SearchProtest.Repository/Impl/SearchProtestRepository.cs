using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Repository.Impl
{
    public class SearchProtestRepository : ISearchProtestRepository
    {
        private readonly IMongoCollection<RequestSearchProtestModel> _collection;
        public SearchProtestRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<RequestSearchProtestModel>("RequestSearchProtest");
        }

        public RequestSearchProtestModel Create(RequestSearchProtestModel entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public List<RequestSearchProtestModel> Get() => _collection.Find<RequestSearchProtestModel>(entity => true).ToList();

        public RequestSearchProtestModel Get(string id) => _collection.Find<RequestSearchProtestModel>(entity => entity.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<RequestSearchProtestModel> GetPendings() => _collection.Find<RequestSearchProtestModel>(t => t.Pending).ToList();

        public void Remove(RequestSearchProtestModel entity) => _collection.DeleteOne(entity => entity.Id.Equals(entity.Id));

        public void Remove(string id) => _collection.DeleteOne(entity => entity.Id.Equals(id));

        public void Update(string id, RequestSearchProtestModel entity) => _collection.ReplaceOne(entity => entity.Id == id, entity);
    }
}
