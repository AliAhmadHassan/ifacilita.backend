using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class SendInviteTextTypeMap: EntityMap<SendInviteTextType>
    {
        internal SendInviteTextTypeMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Subject).ToColumn("subject");
            Map(u => u.Content).ToColumn("content");
        }
    }
}
