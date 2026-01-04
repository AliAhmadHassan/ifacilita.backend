using Com.ByteAnalysis.IFacilita.CertiCadSp.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.Repository.Impl
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
    }
}
