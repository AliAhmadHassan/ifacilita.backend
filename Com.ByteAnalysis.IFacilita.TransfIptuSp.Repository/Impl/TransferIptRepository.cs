using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Repository.Impl
{
    public class TransferIptRepository : ITransferIptuRepository
    {
        private readonly IMongoCollection<RequisitionModel> _collection;

        public TransferIptRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<RequisitionModel>("TransferIptu");
        }

        public RequisitionModel Create(RequisitionModel entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public List<RequisitionModel> Get() => _collection.Find<RequisitionModel>(entity => true).ToList();

        public RequisitionModel Get(string id) => _collection.Find<RequisitionModel>(entity => entity.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<RequisitionModel> GetPendings() => _collection.Find<RequisitionModel>(t => t.Pending).ToList();

        public void Remove(RequisitionModel entity) => _collection.DeleteOne(entity => entity.Id.Equals(entity.Id));

        public void Remove(string id) => _collection.DeleteOne(entity => entity.Id.Equals(id));

        public void Update(string id, RequisitionModel entity) => _collection.ReplaceOne(entity => entity.Id == id, entity);
    }
}
