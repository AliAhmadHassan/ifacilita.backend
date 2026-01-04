using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Repository.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderEntity> _collection;
        public OrderRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<OrderEntity>("Order");
        }

        public OrderEntity Create(OrderEntity entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public List<OrderEntity> Get() => _collection.Find<OrderEntity>(entity => true).ToList();

        public OrderEntity Get(string id) => _collection.Find<OrderEntity>(entity => entity.Id.Equals(id)).FirstOrDefault();

        public OrderEntity Get(string id, CertiticateType type)
        {
            switch (type)
            {
                case CertiticateType.SearchProtest: return _collection.Find<OrderEntity>(x => x.SearchProtest.Id.Equals(id)).FirstOrDefault();
                case CertiticateType.DefectsDefined: return _collection.Find<OrderEntity>(x => x.DefectsDefined.Id.Equals(id)).FirstOrDefault();
                case CertiticateType.TaxDebts: return _collection.Find<OrderEntity>(x => x.TaxDebts.Id.Equals(id)).FirstOrDefault();
                case CertiticateType.IptuDebts:return _collection.Find<OrderEntity>(x => x.IptuDebts.Id.Equals(id)).FirstOrDefault();
                case CertiticateType.PropertyRegistrationData: return _collection.Find<OrderEntity>(x => x.PropertyRegistrationData.Id.Equals(id)).FirstOrDefault();
                case CertiticateType.RealOnus: return _collection.Find<OrderEntity>(x => x.RealOnus.Id.Equals(id)).FirstOrDefault();
                default: return null;
            }
        }

        public void Remove(OrderEntity entity) => _collection.DeleteOne(entity => entity.Id.Equals(entity.Id));

        public void Remove(string id) => _collection.DeleteOne(entity => entity.Id.Equals(id));

        public void Update(string id, OrderEntity entity) => _collection.ReplaceOne(entity => entity.Id == id, entity);

    }
}
