using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class CertificateDefectsDefinedEntity : CertificateBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int ModelCode { get; set; }

        public PersonType PersonType { get; set; }

        public string FullName { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public GenderType GenderType { get; set; }

        public string Email { get; set; }

        public bool Pending { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public DataOrderEntity DataOrder { get; set; }
    }

    public class DataOrderEntity
    {
        public string NumberOrder { get; set; }

        public DateTime DateOrder { get; set; }

        public string UrlCertificate { get; set; }
    }
}
