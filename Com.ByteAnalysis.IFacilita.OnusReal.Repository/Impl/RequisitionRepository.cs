using Com.ByteAnalysis.IFacilita.OnusReal.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Repository.Impl
{
    public class RequisitionRepository : IRequisitionRepository
    {
        private readonly IMongoCollection<Model.Requisition> _requisition;

        public RequisitionRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _requisition = database.GetCollection<Model.Requisition>("requisition");
        }

        public Requisition CreateOrUpdate(Requisition requisition)
        {
            if (String.IsNullOrEmpty(requisition.Id))
            {
                requisition.Request = DateTime.Now;
                _requisition.InsertOne(requisition);
            }
            else
            {
                _requisition.ReplaceOne(requisitionIn => requisitionIn.Id.Equals(requisition.Id), requisition);
            }
            return requisition;
        }

        public Requisition GetUnprocessed() => _requisition.Find<Requisition>(requisition => requisition.StatusProcess == Status.Waiting).FirstOrDefault();

        public List<Requisition> Get() => _requisition.Find<Requisition>(requisition => true).ToList();

        public Requisition Get(string id) => _requisition.Find<Requisition>(requisition => requisition.Id.Equals(id)).FirstOrDefault();

        public void Remove(string id) => _requisition.DeleteOne(requisition => requisition.Id.Equals(id));

        public Requisition Get(int registry, string registration)
            => _requisition.Find<Requisition>(req => req.NumCartorio.Equals(registry) && req.NumMatricola.Equals(registration) && req.s3patch != null).FirstOrDefault();
    }
}
