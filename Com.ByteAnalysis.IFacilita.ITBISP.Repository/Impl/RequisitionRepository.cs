using System;
using System.Collections.Generic;
using System.Text;
using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using MongoDB.Driver;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Repository.Impl
{
    public class RequisitionRepository : IRequisitionRepository
    {
        private readonly IMongoCollection<RequisitionItbiModel> _RequisitionItbiModel;

        public RequisitionRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _RequisitionItbiModel = database.GetCollection<Model.RequisitionItbiModel>("RequisitionItbiModel");
        }

        public RequisitionItbiModel CreateOrUpdate(RequisitionItbiModel RequisitionItbiModel)
        {
            if (String.IsNullOrEmpty(RequisitionItbiModel.Id))
            {
                _RequisitionItbiModel.InsertOne(RequisitionItbiModel);
            }
            else
            {
                _RequisitionItbiModel.ReplaceOne(RequisitionItbiModelIn => RequisitionItbiModelIn.Id.Equals(RequisitionItbiModel.Id), RequisitionItbiModel);
            }
            return RequisitionItbiModel;
        }

        public RequisitionItbiModel GetUnprocessed() => _RequisitionItbiModel.Find<RequisitionItbiModel>(RequisitionItbiModel => RequisitionItbiModel.Pending).FirstOrDefault();

        public List<RequisitionItbiModel> Get() => _RequisitionItbiModel.Find<RequisitionItbiModel>(RequisitionItbiModel => true).ToList();

        public RequisitionItbiModel Get(string id) => _RequisitionItbiModel.Find<RequisitionItbiModel>(RequisitionItbiModel => RequisitionItbiModel.Id.Equals(id)).FirstOrDefault();

        public void Remove(string id) => _RequisitionItbiModel.DeleteOne(RequisitionItbiModel => RequisitionItbiModel.Id.Equals(id));

    }
}
