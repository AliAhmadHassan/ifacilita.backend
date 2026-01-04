using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserSpouseMap: EntityMap<UserSpouse>
    {
        internal UserSpouseMap()
        {
            Map(u => u.SocialSecurityNumber).ToColumn("user_spouse.social_security_number");
            Map(u => u.Name).ToColumn("user_spouse.name");
            Map(u => u.IdentityCard).ToColumn("user_spouse.identity_card");
            Map(u => u.DateOfBirth).ToColumn("user_spouse.date_of_birth");
            Map(u => u.Email).ToColumn("user_spouse.email");
            Map(u => u.MaritalPropertySystems).ToColumn("user_spouse.marital_property_systems");
            Map(u => u.FatherName).ToColumn("user_spouse.father_name");
            Map(u => u.MotherName).ToColumn("user_spouse.mother_name");
        }
    }
}
