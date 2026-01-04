using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class CertificateIptuDebtsEntity : CertificateBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public int IdTransaction { get; set; }

        public string SQL { get; set; }

        public DateTime Request { get; set; }

        public string UrlCallback { get; set; }

        public DateTime Received { get; set; }

        public string UrlCertification { get; set; }

        public DateTime Expiration { get; set; }

        public DateTime CallbackResponse { get; set; }

        public ApiProcessStatus StatusProcess { get; set; }

        public DateTime StatusModified { get; set; }
    }
}
