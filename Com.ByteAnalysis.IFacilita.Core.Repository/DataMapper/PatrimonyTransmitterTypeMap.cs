using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyTransmitterTypeMap: EntityMap<PatrimonyTransmitterType>
    {
        internal PatrimonyTransmitterTypeMap()
        {
            Map(u => u.Id).ToColumn("patrimony_transmitter_type.id");
            Map(u => u.Description).ToColumn("patrimony_transmitter_type.description");
        }
    }
}
