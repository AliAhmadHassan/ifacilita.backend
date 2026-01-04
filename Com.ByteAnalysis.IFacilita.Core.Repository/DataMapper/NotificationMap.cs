using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class NotificationMap: EntityMap<Notification>
    {
        internal NotificationMap()
        {
            Map(u => u.Id).ToColumn("notification.id");
            Map(u => u.Created).ToColumn("notification.created");
            Map(u => u.Message).ToColumn("notification.message");
            Map(u => u.Readed).ToColumn("notification.readed");
            Map(u => u.WhenReaded).ToColumn("notification.when_readed");
            Map(u => u.IdUser).ToColumn("notification.iduser");
        }
    }
}
