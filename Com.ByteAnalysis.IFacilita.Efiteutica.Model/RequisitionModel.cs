using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Model
{
    public class RequisitionModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public long IptuNumber { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCertificate { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public Common.Enumerable.APIStatus Status { get; set; }
    }
}
