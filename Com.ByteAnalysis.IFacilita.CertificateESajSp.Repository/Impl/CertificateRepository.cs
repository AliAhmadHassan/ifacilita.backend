using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Repository.Impl
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly IMongoCollection<ResumeOrderModel> _collection;
        public CertificateRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<ResumeOrderModel>("Certificate");
        }

        public ResumeOrderModel Create(ResumeOrderModel cri)
        {
            _collection.InsertOne(cri);
            return cri;
        }

        public List<ResumeOrderModel> Get() => _collection.Find<ResumeOrderModel>(cri => true).ToList();

        public ResumeOrderModel Get(string id) => _collection.Find<ResumeOrderModel>(cri => cri.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<ResumeOrderModel> GetByDocument(string document)
            => _collection.Find<ResumeOrderModel>(cri => cri.Cpf == document).ToList();

        public IEnumerable<ResumeOrderModel> GetPendings() => _collection.Find<ResumeOrderModel>(t => t.Pending).ToList();

        public void Remove(ResumeOrderModel cri) => _collection.DeleteOne(cri => cri.Id.Equals(cri.Id));

        public void Remove(string id) => _collection.DeleteOne(cri => cri.Id.Equals(id));

        public void Update(string id, ResumeOrderModel cri) => _collection.ReplaceOne(cri => cri.Id == id, cri);
    }
}
