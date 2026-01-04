using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class CertificateRealOnusEntity : CertificateBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public int Registry { get; set; }

        public string Registration { get; set; }

        public Int32 IdUser { get; set; }

        public string Protocol { get; set; }

        public DateTime Request { get; set; }

        public string UrlCallback { get; set; }

        public DateTime Received { get; set; }

        public bool DocumentVisualization { get; set; }

        public string UrlCertification { get; set; }

        public DateTime Expiration { get; set; }

        public DateTime CallbackResponse { get; set; }

        public ApiProcessStatus StatusProcess { get; set; }

        public DateTime StatusModified { get; set; }

        public string s3patch { get; set; }
    }
}
