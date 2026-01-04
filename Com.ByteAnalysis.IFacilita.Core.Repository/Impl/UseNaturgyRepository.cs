using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UseNaturgyRepository : IUserNaturgyRepository
    {

        IDatabaseSettings databaseSettings;

        public UseNaturgyRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id_naturgy
)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
        
                conn.Execute("spd_user_naturgy_data", new {
                    id_naturgy});
            }
        }

        public IEnumerable<UserNaturgy> FindAll()
        {
            IEnumerable<UserNaturgy> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserNaturgy>("sps_user_naturgy_data");
            }
            return entities;
        }

        public UserNaturgy findByIdUser(int id_user)
        {
            UserNaturgy entity = null;

            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserNaturgy>("sps_user_naturgy_data_by_user_pk", param: new { id_user },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }

        public UserNaturgy FindById(int id)
        {
            UserNaturgy entity = null;

            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserNaturgy>("sps_user_naturgy_data_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }

        public IEnumerable<UserNaturgy> FindByIdTreatedRobot(bool treatedRobot)        
        {
            IEnumerable<UserNaturgy> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserNaturgy>("sps_user_naturgy_data_by_treated_robot");
            }
            return entities;
        }

        public UserNaturgy Insert(UserNaturgy entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    change_titulary = entity.ChangeTitulary,
                    password_naturgy = entity.PasswordNaturgy,
                    treated_robot = entity.TreatedRobot,
                    ifacilita_treats = entity.IfacilitaTreats,
                    guid_robot = entity.GuidRobot,
                    id_user = entity.IdUser
                };

                entity.Id = conn.ExecuteScalar<int>("spi_user_naturgy_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserNaturgy Update(UserNaturgy entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    id = entity.Id,
                    change_titulary = entity.ChangeTitulary,
                    password_naturgy = entity.PasswordNaturgy,
                    treated_robot = entity.TreatedRobot,
                    ifacilita_treats = entity.IfacilitaTreats,
                    guid_robot = entity.GuidRobot,
                    id_user = entity.IdUser
                };

                conn.Execute("spu_user_naturgy_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
