using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserProfileRepository : IUserProfileRepository
    {
        IDatabaseSettings databaseSettings;

        public UserProfileRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user_profile", new { id });
            }
        }

        public UserProfile Insert(UserProfile entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name,
                    description = entity.Description
                };

                entity.Id = conn.ExecuteScalar<int>("spi_user_profile", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserProfile Update(UserProfile entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name,
                    description = entity.Description
                };

                conn.Execute("spu_user_profile", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<UserProfile> FindAll()
        {
            IEnumerable<UserProfile> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserProfile>("sps_user_profile");
            }
            return entities;
        }

        public UserProfile FindById(int id)
        {
            UserProfile entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserProfile>("sps_user_profile_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}