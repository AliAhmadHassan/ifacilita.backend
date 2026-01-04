using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public IMongoCollection<T> Collection { get; set; }

        public RepositoryBase(IMongoDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAsync() => await Collection.Find<T>(t => true).ToListAsync();

        public async Task<T> GetAsync(string id) => await Collection.Find<T>(t => t.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task RemoveAsync(T entity) => await Collection.DeleteOneAsync(transaction => transaction.Id.Equals(entity.Id));

        public async Task RemoveAsync(string id) => await Collection.DeleteOneAsync(transaction => transaction.Id.Equals(id));

        public async Task UpdateAsync(string id, T entity) => await Collection.ReplaceOneAsync(transaction => transaction.Id == id, entity);

        public void SetNameCollection(string nameCollection)
        {
            Collection = _database.GetCollection<T>(nameCollection);
        }
    }
}
