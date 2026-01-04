using Com.ByteAnalysis.IFacilita.IptuDebit.Model;
using MongoDB.Driver;
using System;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Repository.Impl
{
    public class RequisitionRepository : IRequisitionRepository
    {
        private readonly IMongoCollection<Model.Requisition> _requisition;

        public RequisitionRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _requisition = database.GetCollection<Model.Requisition>("requisitions");
        }

        public Requisition CreateOrUpdate(Requisition requisition)
        {
            if (String.IsNullOrEmpty(requisition.Id))
            {
                requisition.Request = DateTime.Now;
                _requisition.InsertOne(requisition);
            } else
            {
                _requisition.ReplaceOne(requisitionIn => requisitionIn.Id.Equals(requisition.Id), requisition);
            }
            return requisition;
        }

        public Requisition Get(string id) => _requisition.Find<Requisition>(entity => entity.Id.Equals(id)).FirstOrDefault();

        public Requisition GetUnprocessed() => _requisition.Find<Requisition>(requisition => requisition.StatusProcess == Status.Waiting).FirstOrDefault();


    }
}
