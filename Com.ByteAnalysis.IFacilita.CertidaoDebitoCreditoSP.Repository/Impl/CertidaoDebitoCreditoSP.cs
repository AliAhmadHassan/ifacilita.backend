using Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Repository.Impl
{
    public class CertidaoDebitoCreditoSP : ICertidaoDebitoCreditoSPRepository
    {
        private readonly IMongoCollection<Model.CertidaoDebitoCreditoSP> _repository;

        public CertidaoDebitoCreditoSP(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _repository = database.GetCollection<Model.CertidaoDebitoCreditoSP>("CertidoesDebitoCreditoSP");
        }

        public Model.CertidaoDebitoCreditoSP Create(Model.CertidaoDebitoCreditoSP certidao)
        {
            _repository.InsertOne(certidao);
            return certidao;
        }

        public List<Model.CertidaoDebitoCreditoSP> Get()
            => _repository.Find<Model.CertidaoDebitoCreditoSP>(certidao => true).ToList();

        public Model.CertidaoDebitoCreditoSP Get(string id) =>
            _repository.Find<Model.CertidaoDebitoCreditoSP>(certidao => certidao.Id.Equals(id)).FirstOrDefault();

        public void Remove(string id) => _repository.DeleteOne(certidao => certidao.Id.Equals(id));

        public Model.CertidaoDebitoCreditoSP GetUnprocessed() 
            => _repository.Find<Model.CertidaoDebitoCreditoSP>(certidao => certidao.StatusProcess == Status.Waiting).FirstOrDefault();

        public Model.CertidaoDebitoCreditoSP CreateOrUpdate(Model.CertidaoDebitoCreditoSP certidao)
        {
            if (String.IsNullOrEmpty(certidao.Id))
            {
                certidao.DateInsert = DateTime.Now;
                _repository.InsertOne(certidao);
            }
            else
            {
                _repository.ReplaceOne(requisitionIn => requisitionIn.Id.Equals(certidao.Id), certidao);
            }
            return certidao;
        }
    }
}
