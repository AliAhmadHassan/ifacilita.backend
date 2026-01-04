using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class UserBankDataRepository : IUserBankDataRepository
    {
        IDatabaseSettings databaseSettings;

        public UserBankDataRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_user_bank_data", new { id });
            }
        }

        public UserBankData Insert(UserBankData entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    agency = entity.Agency,
                    account = entity.Account,
                    account_digit = entity.AccountDigit
                };

                entity.Id = conn.ExecuteScalar<int>("spi_user_bank_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public UserBankData Update(UserBankData entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    agency = entity.Agency,
                    account = entity.Account,
                    account_digit = entity.AccountDigit
                };

                conn.Execute("spu_user_bank_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<UserBankData> FindAll()
        {
            IEnumerable<UserBankData> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<UserBankData>("sps_user_bank_data");
            }
            return entities;
        }

        public UserBankData FindById(int id)
        {
            UserBankData entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<UserBankData>("sps_user_bank_data_by_pk", param: new { id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
    }
}