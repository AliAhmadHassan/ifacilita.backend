using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserPushNotificationRepository : IUserPushNotificationRepository
    {
        IDatabaseSettings databaseSettings;

        public UserPushNotificationRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public IEnumerable<UserPushNotification> GetByIdUser(int idUser)
        {
            IEnumerable<UserPushNotification> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserPushNotification>("sps_user_push_notification_by_iduser", new { iduser = idUser}, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entities;
        }

        public UserPushNotification Insert(UserPushNotification userPushNotification)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    iduser = userPushNotification.IdUser,
                    token = userPushNotification.Token
                };

                userPushNotification.Id = conn.ExecuteScalar<int>("spi_user_push_notification", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return userPushNotification;
        }
    }
}
