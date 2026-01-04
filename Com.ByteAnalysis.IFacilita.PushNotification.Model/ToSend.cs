using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class ToSend
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public Message Message { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }

    }
}
