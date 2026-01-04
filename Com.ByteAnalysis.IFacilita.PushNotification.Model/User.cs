using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<String> Tokens { get; set; }
        public List<Sended> Sendeds { get; set; }

    }
}
