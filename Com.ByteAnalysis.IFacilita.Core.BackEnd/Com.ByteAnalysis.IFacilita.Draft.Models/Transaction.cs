using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Draft.Model
{
    public class Transaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public Patrimony Patrimony { get; set; }
        public Seller Seller { get; set; }
        public Buyer Buyer { get; set; }
        public decimal Value { get; set; }
        public string FormOfPayment { get; set; }
        public string Document { get; set; }
    }
}
