using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Mcri.Model
{
    public class CriModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        public string Iptu { get; set; }

        public bool NewImmobile { get; set; }

        public string AddressNewImmobile { get; set; }

        public IEnumerable<PurchaserModel> Purchasers { get; set; }

        public IEnumerable<TransmittingModel> Transmittings { get; set; }

        public AcquisitionTitle AcquisitionTitle { get; set; }

        public FractionModel Fraction { get; set; }

        public DeliveryGuideIptuModel DeliveryGuide { get; set; }

        public ProtocolModel Protocol { get; set; }

        public bool Pending { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public Common.Enumerable.APIStatus Status { get; set; }

    }
}
