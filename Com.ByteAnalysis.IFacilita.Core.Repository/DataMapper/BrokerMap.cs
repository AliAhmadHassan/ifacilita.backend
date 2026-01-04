using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class BrokerMap: EntityMap<Broker>
    {
        internal BrokerMap()
        {
            Map(u => u.RegistrationNumber).ToColumn("broker.registration_number");
        }
    }
}
