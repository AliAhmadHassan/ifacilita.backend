using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class SendInviteTextTypeRepository : ISendInviteTextTypeRepository
    {
        IDatabaseSettings databaseSettings;

        public SendInviteTextTypeRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_send_invite_text_type", new { id });
            }
        }

        public SendInviteTextType Insert(SendInviteTextType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    subject = entity.Subject,
                    content = entity.Content
                };

                entity.Id = conn.ExecuteScalar<int>("spi_send_invite_text_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public SendInviteTextType Update(SendInviteTextType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    subject = entity.Subject,
                    content = entity.Content
                };

                conn.Execute("spu_send_invite_text_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<SendInviteTextType> FindAll()
        {
            IEnumerable<SendInviteTextType> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<SendInviteTextType>("sps_send_invite_text_type");
            }
            return entities;
        }

        public SendInviteTextType FindById(int id)
        {
            SendInviteTextType entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<SendInviteTextType>("sps_send_invite_text_type_by_pk", new { id });
            }
            return entity;
        }
    }
}