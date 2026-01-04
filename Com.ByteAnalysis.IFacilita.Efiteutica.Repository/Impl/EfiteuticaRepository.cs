using Com.ByteAnalysis.IFacilita.Efiteutica.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Repository.Impl
{
    public class EfiteuticaRepository : IEfiteuticaRepository
    {
        private readonly IMongoCollection<RequisitionModel> _collection;

        public EfiteuticaRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<RequisitionModel>("EfiteuticaCollection");
        }

        public async Task<RequisitionModel> CreateAsync(RequisitionModel req)
        {
            req.CreateAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(req);
            return req;
        }

        public async Task<List<RequisitionModel>> GetAsync()
            => await _collection.Find<RequisitionModel>(cri => true).ToListAsync();

        public async Task<RequisitionModel> GetAsync(string id)
            => await _collection.Find<RequisitionModel>(cri => cri.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<RequisitionModel>> GetPendingsAsync()
            => await _collection.Find<RequisitionModel>(req => req.Status == Common.Enumerable.APIStatus.Pending).ToListAsync();

        public async Task RemoveAsync(RequisitionModel req)
             => await _collection.DeleteOneAsync(x => x.Id.Equals(req.Id));

        public async Task RemoveAsync(string id)
             => await _collection.DeleteOneAsync(x => x.Id.Equals(id));

        public async Task UpdateAsync(string id, RequisitionModel req)
        {
            var current = await GetAsync(id);
            req.CreateAt = current.CreateAt;
            req.UpdateAt = DateTime.UtcNow;

            await _collection.ReplaceOneAsync(x => x.Id == id, req);
        }
            
    }
}
