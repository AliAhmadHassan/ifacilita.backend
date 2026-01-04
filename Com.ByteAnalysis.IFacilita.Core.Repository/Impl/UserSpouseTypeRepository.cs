using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserSpouseTypeRepository : IUserSpouseTypeRepository
    {
        IDatabaseSettings databaseSettings;

        public UserSpouseTypeRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user_spouse_type", new { id });
            }
        }

        public UserSpouseType Insert(UserSpouseType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    name = entity.Name
                };

                entity.Id = conn.ExecuteScalar<int>("spi_user_spouse_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserSpouseType Update(UserSpouseType entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    name = entity.Name
                };

                conn.Execute("spu_user_spouse_type", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<UserSpouseType> FindAll()
        {
            IEnumerable<UserSpouseType> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserSpouseType>("sps_user_spouse_type");
            }
            return entities;
        }

        public UserSpouseType FindById(int id)
        {
            UserSpouseType entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserSpouseType>("sps_user_spouse_type_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}