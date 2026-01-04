using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public class CertificateTaxDebtsEntity : CertificateBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public String CpfCnpj { get; set; }

        public String Nome { get; set; }

        public bool PessoaFisica { get; set; }

        public int IdUserIfacilita { get; set; }

        public String IdDocS3 { get; set; }

        public DateTime DateInsert { get; set; }

        public ApiProcessStatus StatusProcess { get; set; }

        public DateTime StatusModified { get; set; }

        public string UrlCallback { get; set; }
    }
}
