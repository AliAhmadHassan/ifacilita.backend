using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class RegistryMap: EntityMap<Registry>
    {
        internal RegistryMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Name).ToColumn("name");
            Map(u => u.IdAddress).ToColumn("idaddress");
        }
    }
}
