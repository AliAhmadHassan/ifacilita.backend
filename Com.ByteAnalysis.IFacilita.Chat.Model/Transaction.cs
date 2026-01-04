using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Chat.Model
{
    public class Transaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public List<User> Users { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}
