using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class SendInviteMap: EntityMap<SendInvite>
    {
        internal SendInviteMap()
        {
            Map(u => u.Id).ToColumn("send_invite.id");
            Map(u => u.Created).ToColumn("send_invite.created");
            Map(u => u.EMailSended).ToColumn("send_invite.e_mail_sended");
            Map(u => u.SMSSended).ToColumn("send_invite.s_m_s_sended");
            Map(u => u.WhatsappSended).ToColumn("send_invite.whatsapp_sended");
            Map(u => u.PushSended).ToColumn("send_invite.push_sended");
            Map(u => u.RealEstateRegisteredNumber).ToColumn("send_invite.real_estate_registered_number");
            Map(u => u.IdUser).ToColumn("send_invite.iduser");
            Map(u => u.IdSendInviteTextType).ToColumn("send_invite.idsend_invite_text_type");
        }
    }
}
