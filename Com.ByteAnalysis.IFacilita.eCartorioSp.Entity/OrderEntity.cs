using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class OrderEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public decimal Value { get; set; }

        public CertificateSearchProtestEntity SearchProtest { get; set; }

        public CertificateDefectsDefinedEntity DefectsDefined { get; set; }

        public CertificateTaxDebtsEntity TaxDebts { get; set; }

        public CertificateIptuDebtsEntity IptuDebts { get; set; }

        public CertificatePropertyRegistrationDataEntity PropertyRegistrationData { get; set; }

        public CertificateRealOnusEntity RealOnus { get; set; }


    }
}
