using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class SendInviteMap: EntityMap<SendInvite>
    {
        internal SendInviteMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Created).ToColumn("created");
            Map(u => u.EMailSended).ToColumn("e_mail_sended");
            Map(u => u.SMSSended).ToColumn("s_m_s_sended");
            Map(u => u.WhatsappSended).ToColumn("whatsapp_sended");
            Map(u => u.PushSended).ToColumn("push_sended");
            Map(u => u.RealEstateRegisteredNumber).ToColumn("real_estate_registered_number");
            Map(u => u.IdUser).ToColumn("iduser");
            Map(u => u.IdSendInviteTextType).ToColumn("idsend_invite_text_type");
        }
    }
}
