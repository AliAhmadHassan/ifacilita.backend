using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class SendInviteTextTypeMap: EntityMap<SendInviteTextType>
    {
        internal SendInviteTextTypeMap()
        {
            Map(u => u.Id).ToColumn("send_invite_text_type.id");
            Map(u => u.Subject).ToColumn("send_invite_text_type.subject");
            Map(u => u.Content).ToColumn("send_invite_text_type.content");
        }
    }
}
