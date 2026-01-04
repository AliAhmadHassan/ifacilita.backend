using Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Data.Repositories
{
    public class GuideRequestRepository : IGuideRequestRepository
    {
        private readonly IMongoCollection<Domain.Entities.GuideRequest> _collection;

        public GuideRequestRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<Domain.Entities.GuideRequest>("guiderequest");
        }

        public async Task<Domain.Entities.GuideRequest> CreateAsync(Domain.Entities.GuideRequest guideRequest)
        {
            await _collection.InsertOneAsync(guideRequest);
            return guideRequest;
        }

        public async Task<List<Domain.Entities.GuideRequest>> GetAsync() => await _collection.Find<Domain.Entities.GuideRequest>(t => true).ToListAsync();

        public async Task<Domain.Entities.GuideRequest> GetAsync(string id) => await _collection.Find<Domain.Entities.GuideRequest>(t => t.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<IEnumerable<Domain.Entities.GuideRequest>> GetGuidePendingsAsync()
            => await _collection.Find<Domain.Entities.GuideRequest>(t => t.StatusGuide.Equals(1) && t.Protocol != null).ToListAsync();

        public async Task<IEnumerable<Domain.Entities.GuideRequest>> GetPendingsAsync() => await _collection.Find<Domain.Entities.GuideRequest>(t => t.Status.Equals(1)).ToListAsync();

        public async Task RemoveAsync(Domain.Entities.GuideRequest guideRequest) => await _collection.DeleteOneAsync(transaction => transaction.Id.Equals(guideRequest.Id));

        public async Task RemoveAsync(string id) => await _collection.DeleteOneAsync(transaction => transaction.Id.Equals(id));

        public async Task UpdateAsync(string id, Domain.Entities.GuideRequest guideRequest) => await _collection.ReplaceOneAsync(transaction => transaction.Id == id, guideRequest);
    }
}
