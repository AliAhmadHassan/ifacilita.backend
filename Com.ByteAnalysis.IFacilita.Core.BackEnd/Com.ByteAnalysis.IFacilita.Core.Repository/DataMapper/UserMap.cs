using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserMap: EntityMap<User>
    {
        internal UserMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.SocialSecurityNumber).ToColumn("social_security_number");
            Map(u => u.Name).ToColumn("name");
            Map(u => u.Password).ToColumn("password");
            Map(u => u.EMail).ToColumn("e_mail");
            Map(u => u.DDI).ToColumn("ddi");
            Map(u => u.DDD).ToColumn("ddd");
            Map(u => u.MobileNumber).ToColumn("mobile_number");
            Map(u => u.IdentityCard).ToColumn("identity_card");
            Map(u => u.DateOfBirth).ToColumn("date_of_birth");
            Map(u => u.IdUserProfile).ToColumn("iduser_profile");
            Map(u => u.IdAddress).ToColumn("idaddress");
            Map(u => u.BrokerRegistrationNumber).ToColumn("broker_registration_number");
            Map(u => u.IdUserSpouseType).ToColumn("iduser_spouse_type");
            Map(u => u.UserSpouseSocialSecurityNumber).ToColumn("user_spouse_social_security_number");
            Map(u => u.IdUserBankData).ToColumn("iduser_bank_data");
        }
    }
}
