using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class CityMap: EntityMap<City>
    {
        internal CityMap()
        {
            Map(u => u.Sig).ToColumn("sig");
            Map(u => u.Description).ToColumn("description");
        }
    }
}
