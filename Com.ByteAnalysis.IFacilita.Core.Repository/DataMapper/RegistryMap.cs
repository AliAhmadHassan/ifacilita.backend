using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class RegistryMap: EntityMap<Registry>
    {
        internal RegistryMap()
        {
            Map(u => u.Id).ToColumn("registry.id");
            Map(u => u.Name).ToColumn("registry.name");
            Map(u => u.Email1).ToColumn("registry.email_1");
            Map(u => u.Email2).ToColumn("registry.email_2");
            Map(u => u.Email3).ToColumn("registry.email_3");
            Map(u => u.IdAddress).ToColumn("registry.idaddress");
            Map(u => u.City).ToColumn("registry.city");
        }
    }
}
