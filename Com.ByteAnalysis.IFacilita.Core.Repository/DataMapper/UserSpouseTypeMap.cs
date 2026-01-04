using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserSpouseTypeMap: EntityMap<UserSpouseType>
    {
        internal UserSpouseTypeMap()
        {
            Map(u => u.Id).ToColumn("user_spouse_type.id");
            Map(u => u.Name).ToColumn("user_spouse_type.name");
        }
    }
}
