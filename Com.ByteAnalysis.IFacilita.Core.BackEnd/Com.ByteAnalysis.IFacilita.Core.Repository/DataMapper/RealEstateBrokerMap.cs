using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class RealEstateBrokerMap: EntityMap<RealEstateBroker>
    {
        internal RealEstateBrokerMap()
        {
            Map(u => u.RealEstateRegisteredNumber).ToColumn("real_estate_registered_number");
            Map(u => u.BrokerRegistrationNumber).ToColumn("broker_registration_number");
        }
    }
}
