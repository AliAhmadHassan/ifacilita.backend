using Com.ByteAnalysis.IFacilita.RGI.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Repository.Impl
{
    public class RequisitionRepository : IRequisitionRepository
    {
        private readonly IMongoCollection<Model.Requisition> _requisition;

        public RequisitionRepository(Model.IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _requisition = database.GetCollection<Model.Requisition>("rgi");
        }

        public Model.Requisition CreateOrUpdate(Model.Requisition requisition)
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

        public IEnumerable<Requisition> Get() => _requisition.Find<Model.Requisition>(x=>true).ToList();

        public Model.Requisition GetById(string id) => _requisition.Find<Model.Requisition>(requisition => requisition.Id.Equals(id)).FirstOrDefault();

        public Model.Requisition GetUnprocessed() => _requisition.Find<Model.Requisition>(requisition => requisition.RpaStatus == Model.Status.Waiting).FirstOrDefault();
    }
}
