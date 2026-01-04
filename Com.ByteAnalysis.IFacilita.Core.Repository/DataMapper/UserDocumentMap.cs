using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserDocumentMap: EntityMap<UserDocument>
    {
        internal UserDocumentMap()
        {
            Map(u => u.IdUser).ToColumn("iduser");
            Map(u => u.IdentityCard).ToColumn("identity_card");
            Map(u => u.SocialSecurityNumber).ToColumn("social_security_number");
            Map(u => u.SpouseIdentityCard).ToColumn("spouse_identity_card");
            Map(u => u.SpouseSocialSecurityNumber).ToColumn("spouse_social_security_number");
            Map(u => u.MarriageCertificate).ToColumn("marriage_certificate");
        }
    }
}
