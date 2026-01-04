using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Model
{
    public class Requisition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public int NumCartorio { get; set; }

        public string NumMatricola { get; set; }

        public Int32 IdUser { get; set; }

        public string Protocolo { get; set; }

        public DateTime? Request { get; set; }

        public string UrlCallback { get; set; }

        public DateTime? Received { get; set; }

        /// <summary>
        /// Extrair a visualização ou a Certidão
        /// </summary>
        public bool DocumentVisualization { get; set; }

        public string UrlCertification { get; set; }

        public DateTime? Expiration { get; set; }

        public DateTime? CallbackResponse { get; set; }

        public Status StatusProcess { get; set; }

        public DateTime? StatusModified { get; set; }

        public string s3patch { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
