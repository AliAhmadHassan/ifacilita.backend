using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Model
{
    public class ResumeOrderModel
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

        public DataOrderModel DataOrder { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public APIStatus Status { get; set; }
    }
}
