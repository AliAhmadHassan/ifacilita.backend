using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Model
{
    public class OwnershipModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string NameClient { get; set; }

        public string CodeClient { get; set; }

        public TypeClient TypeClient { get; set; }

        public PhysicalPersonModel PhysicalPerson { get; set; }

        public LegalPersonModel LegalPerson { get; set; }

        public string CelPhone { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string Neighborhood { get; set; }

        public string City { get; set; }

        public string Uf { get; set; }

        public string Cep { get; set; }

        public DateTime AutoReadingDate { get; set; }

        public string AutoReadingValue { get; set; }

        public TypeActivity TypeActivity { get; set; }

        public bool Pending { get; set; }

        public IEnumerable<AttachmentModel> Attachments { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public APIStatus Status { get; set; }

    }
}
