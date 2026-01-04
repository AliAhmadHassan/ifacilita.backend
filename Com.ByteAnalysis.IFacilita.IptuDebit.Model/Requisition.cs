using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Model
{
    public class Requisition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        
        public int IdTransaction { get; set; }
        
        public string SQL { get; set; }
        
        public DateTime? Request { get; set; }
        
        public string UrlCallback { get; set; } //https://ifacilita.com/api/transaction/{id}/Callback-itbi-rj     replace("{id}", _id);
        
        public DateTime? Received { get; set; }
        
        public string UrlCertification { get; set; }
        
        public DateTime? Expiration { get; set; }
        
        public DateTime? CallbackResponse { get; set; }
        
        public Status StatusProcess { get; set; }
        
        public DateTime? StatusModified { get; set; }
        
        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
