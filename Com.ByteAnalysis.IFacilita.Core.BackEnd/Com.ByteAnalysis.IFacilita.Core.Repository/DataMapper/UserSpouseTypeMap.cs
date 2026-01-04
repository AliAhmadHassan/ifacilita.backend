using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserSpouseTypeMap: EntityMap<UserSpouseType>
    {
        internal UserSpouseTypeMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Name).ToColumn("name");
        }
    }
}
