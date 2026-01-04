using Com.ByteAnalysis.IFacilita.PushNotification.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Repository.Impl
{
    public class ToSendRepository : IToSendRepository
    {
        private readonly IMongoCollection<Model.ToSend> _toSends;
        

        public ToSendRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _toSends = database.GetCollection<Model.ToSend>("to_sends");
        }

        public ToSend CreateOrUpdate(ToSend toSend)
        {
            if (toSend.Id == null || toSend.Id == "")
                _toSends.InsertOne(toSend);
            else
                _toSends.ReplaceOne(ts=>ts.Id.Equals(toSend.Id), toSend);

            throw new NotImplementedException();
        }

        public ToSend Get(string id) => _toSends.Find<Model.ToSend>(ts => ts.Id.Equals(id)).FirstOrDefault();

        public IEnumerable<ToSend> Get() => _toSends.Find<Model.ToSend>(ts => true).ToEnumerable();

        public void Remove(string id) => _toSends.DeleteOne<Model.ToSend>(ts => ts.Id.Equals(id));
    }
}
