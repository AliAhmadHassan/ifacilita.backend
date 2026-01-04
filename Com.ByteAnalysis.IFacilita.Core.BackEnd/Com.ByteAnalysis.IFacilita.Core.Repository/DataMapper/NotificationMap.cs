using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class NotificationMap: EntityMap<Notification>
    {
        internal NotificationMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Created).ToColumn("created");
            Map(u => u.Message).ToColumn("message");
            Map(u => u.Readed).ToColumn("readed");
            Map(u => u.WhenReaded).ToColumn("when_readed");
            Map(u => u.IdUser).ToColumn("iduser");
        }
    }
}
