using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities
{
    public class EntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
