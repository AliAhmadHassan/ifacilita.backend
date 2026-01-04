using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Repository.Impl
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly IMongoCollection<CertificateEntity> _collection;
        public CertificateRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<CertificateEntity>("Certificate");
        }

        public CertificateEntity Create(CertificateEntity entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public List<CertificateEntity> Get() => _collection.Find<CertificateEntity>(entity => true).ToList();

        public CertificateEntity Get(string id) => _collection.Find<CertificateEntity>(entity => entity.Id.Equals(id)).FirstOrDefault();

        public void Remove(CertificateEntity entity) => _collection.DeleteOne(entity => entity.Id.Equals(entity.Id));

        public void Remove(string id) => _collection.DeleteOne(entity => entity.Id.Equals(id));

        public void Update(string id, CertificateEntity entity) => _collection.ReplaceOne(entity => entity.Id == id, entity);


    }
}
