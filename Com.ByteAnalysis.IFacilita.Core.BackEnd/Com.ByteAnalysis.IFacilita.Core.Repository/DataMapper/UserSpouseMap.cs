using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserSpouseMap: EntityMap<UserSpouse>
    {
        internal UserSpouseMap()
        {
            Map(u => u.SocialSecurityNumber).ToColumn("social_security_number");
            Map(u => u.Name).ToColumn("name");
            Map(u => u.IdentityCard).ToColumn("identity_card");
            Map(u => u.DateOfBirth).ToColumn("date_of_birth");
        }
    }
}
