using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.Model
{
    public class CertidaoDebitoCreditoSP
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public String CpfCnpj { get; set; }

        public String Nome { get; set; }

        public bool PessoaFisica { get; set; }

        public int IdUserIfacilita { get; set; }

        public String IdDocS3 { get; set; }

        public DateTime DateInsert { get; set; }

        public Status StatusProcess { get; set; }

        public DateTime StatusModified { get; set; }

        public string UrlCallback { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
