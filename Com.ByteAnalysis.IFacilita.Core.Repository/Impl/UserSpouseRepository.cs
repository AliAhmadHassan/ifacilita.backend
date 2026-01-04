using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserSpouseRepository : IUserSpouseRepository
    {
        IDatabaseSettings databaseSettings;

        public UserSpouseRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(long id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user_spouse", new { id });
            }
        }

        public UserSpouse Insert(UserSpouse entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    social_security_number = entity.SocialSecurityNumber,
                    name = entity.Name,
                    identity_card = entity.IdentityCard,
                    date_of_birth = entity.DateOfBirth,
                    email = entity.Email,
                    marital_property_systems = entity.MaritalPropertySystems,
                    father_name = entity.FatherName,
                    mother_name = entity.MotherName
                };

                conn.Execute("spi_user_spouse", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserSpouse Update(UserSpouse entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    social_security_number = entity.SocialSecurityNumber,
                    name = entity.Name,
                    identity_card = entity.IdentityCard,
                    date_of_birth = entity.DateOfBirth,
                    email = entity.Email,
                    marital_property_systems = entity.MaritalPropertySystems,
                    father_name = entity.FatherName,
                    mother_name = entity.MotherName
                };

                conn.Execute("spu_user_spouse", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<UserSpouse> FindAll()
        {
            IEnumerable<UserSpouse> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserSpouse>("sps_user_spouse");
            }
            return entities;
        }

        public UserSpouse FindById(long id)
        {
            UserSpouse entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserSpouse>("sps_user_spouse_by_pk", param: new { social_security_number = id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}