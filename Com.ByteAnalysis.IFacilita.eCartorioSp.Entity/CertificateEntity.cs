using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class CertificateEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public CertiticateType CertiticateType { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }
    }
}
