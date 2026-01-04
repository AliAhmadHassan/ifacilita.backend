using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserProfileMap: EntityMap<UserProfile>
    {
        internal UserProfileMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Name).ToColumn("name");
            Map(u => u.Description).ToColumn("description");
        }
    }
}
