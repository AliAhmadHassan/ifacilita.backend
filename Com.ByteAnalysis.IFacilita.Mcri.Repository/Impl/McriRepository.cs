using Com.ByteAnalysis.IFacilita.Mcri.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Mcri.Repository.Impl
{
    public class McriRepository : IMcriRepository
    {
        private readonly IMongoCollection<CriModel> _collection;
        public McriRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<CriModel>("Cri");
        }

        public CriModel Create(CriModel cri)
        {
            _collection.InsertOne(cri);
            return cri;
        }

        public List<CriModel> Get() => _collection.Find<CriModel>(cri => true).ToList();

        public CriModel Get(string id) => _collection.Find<CriModel>(cri => cri.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<CriModel> GetPendings() => _collection.Find<CriModel>(t => t.Pending.Equals(1)).ToList();

        public void Remove(CriModel cri) => _collection.DeleteOne(cri => cri.Id.Equals(cri.Id));

        public void Remove(string id) => _collection.DeleteOne(cri => cri.Id.Equals(id));

        public void Update(string id, CriModel cri) => _collection.ReplaceOne(cri => cri.Id == id, cri);
    }
}
