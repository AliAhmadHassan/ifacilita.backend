using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserMap: EntityMap<User>
    {
        internal UserMap()
        {
            Map(u => u.Id).ToColumn("user.id");
            Map(u => u.SocialSecurityNumber).ToColumn("user.social_security_number");
            Map(u => u.Name).ToColumn("user.name");
            Map(u => u.LastName).ToColumn("user.last_name");
            Map(u => u.Password).ToColumn("user.password");
            Map(u => u.EMail).ToColumn("user.e_mail");
            Map(u => u.DDI).ToColumn("user.ddi");
            Map(u => u.DDD).ToColumn("user.ddd");
            Map(u => u.MobileNumber).ToColumn("user.mobile_number");
            Map(u => u.IdentityCard).ToColumn("user.identity_card");
            Map(u => u.DateOfBirth).ToColumn("user.date_of_birth");
            Map(u => u.SocialLoginAuthorizationCode).ToColumn("user.social_login_authorization_code");
            Map(u => u.iddefailtTransaction).ToColumn("user.iddefailt_transaction");
            Map(u => u.IdUserProfile).ToColumn("user.iduser_profile");
            Map(u => u.IdAddress).ToColumn("user.idaddress");
            Map(u => u.BrokerRegistrationNumber).ToColumn("user.broker_registration_number");
            Map(u => u.IdUserSpouseType).ToColumn("user.iduser_spouse_type");
            Map(u => u.UserSpouseSocialSecurityNumber).ToColumn("user.user_spouse_social_security_number");
            Map(u => u.IdUserBankData).ToColumn("user.iduser_bank_data");
            Map(u => u.Nationality).ToColumn("user.nationality");
            Map(u => u.PushNotification).ToColumn("user.push_notification");
            Map(u => u.FatherName).ToColumn("user.father_name");
            Map(u => u.MotherName).ToColumn("user.mother_name");
            Map(u => u.PaymentClientId).ToColumn("user.payment_clientid");
            Map(u => u.PaymentExtenalClientId).ToColumn("user.payment_clientexternalid");
        }
    }
}
