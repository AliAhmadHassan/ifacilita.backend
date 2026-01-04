using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Model
{
    public class Requisition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public int IdTransaction { get; set; }
        public DateTime Request { get; set; }
        public string UrlCallback { get; set; }
        public DateTime Received { get; set; }
        public DateTime CallbackResponse { get; set; }
        public Status RpaStatus { get; set; }
        public DateTime StatusModified { get; set; }
        public ShipmentData ShipmentData { get; set; }
        public EPropocolo EPropocolo { get; set; }

        public Common.Enumerable.APIStatus Status { get; set; }
        public IEnumerable<Common.Exceptions.GlobalError> Errors { get; set; }

    }
}
