using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class RealEstateBroker: BasicEntity
    {
        public Int64? RealEstateRegisteredNumber { get; set; }
        public String? BrokerRegistrationNumber { get; set; }
    }
}