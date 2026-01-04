using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserProfileMap: EntityMap<UserProfile>
    {
        internal UserProfileMap()
        {
            Map(u => u.Id).ToColumn("user_profile.id");
            Map(u => u.Name).ToColumn("user_profile.name");
            Map(u => u.Description).ToColumn("user_profile.description");
        }
    }
}
