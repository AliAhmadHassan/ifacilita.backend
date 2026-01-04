using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserPushNotificationMap : EntityMap<UserPushNotification>
    {
        internal UserPushNotificationMap()
        {
            Map(u => u.Id).ToColumn("user_push_notification.id");
            Map(u => u.IdUser).ToColumn("user_push_notification.iduser");
            Map(u => u.Token).ToColumn("user_push_notification.token");
        }
    }
}
