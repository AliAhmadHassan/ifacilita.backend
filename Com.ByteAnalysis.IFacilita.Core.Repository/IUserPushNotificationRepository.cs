using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IUserPushNotificationRepository
    {
        IEnumerable<Entity.UserPushNotification> GetByIdUser(int idUser);

        Entity.UserPushNotification Insert(Entity.UserPushNotification userPushNotification);
    }
}
