using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Entities
{
    public class GuideRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public int Iptu { get; set; }

        public decimal Value { get; set; }

        public string TransactionNature { get; set; }

        public string Pal { get; set; }

        public string TransferredPart { get; set; }

        public int Status { get; set; }

        public int StatusGuide { get; set; }

        public bool Approved { get; set; }

        public Simulate Simulate { get; set; }

        public Generation Generation { get; set; }

        public PurchaserTransmitted PurchaserTransmitted { get; set; }

        public PreProtocol PreProtocol { get; set; }

        public Protocol Protocol { get; set; }

        public Guide Guide { get; set; }

        public IEnumerable<Common.Exceptions.GlobalError> Errors { get; set; }

        public string UrlCallback { get; set; } //https://ifacilita.com/api/transaction/{0}/Callback-itbi-rj     UrlCallback = stringFormat(UrlCallback, _id);

        public string UrlCallbackResponse { get; set; }

    }
}
