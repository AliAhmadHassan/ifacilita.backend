using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class NotificationRepository : INotificationRepository
    {
        IDatabaseSettings databaseSettings;

        public NotificationRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_notification", new { id });
            }
        }

        public Notification Insert(Notification entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    created = entity.Created,
                    message = entity.Message,
                    readed = entity.Readed,
                    when_readed = entity.WhenReaded,
                    iduser = entity.IdUser
                };

                entity.Id = conn.ExecuteScalar<int>("spi_notification", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Notification Update(Notification entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    created = entity.Created,
                    message = entity.Message,
                    readed = entity.Readed,
                    when_readed = entity.WhenReaded,
                    iduser = entity.IdUser
                };

                conn.Execute("spu_notification", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Notification> FindAll()
        {
            IEnumerable<Notification> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Notification>("sps_notification");
            }
            return entities;
        }

        public Notification FindById(int id)
        {
            Notification entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Notification>("sps_notification_by_pk", new { id });
            }
            return entity;
        }
		public List<Notification> FindByIdUser (Int32 IdUser){throw new NotImplementedException();}
    }
}