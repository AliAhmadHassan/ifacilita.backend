using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class PlatformBilletBankDataRepository : IPlatformBilletBankDataRepository
    {
        IDatabaseSettings databaseSettings;

        public PlatformBilletBankDataRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_platform_billet_bank_data", new { id });
            }
        }

        public PlatformBilletBankData Insert(PlatformBilletBankData entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    agency = entity.Agency,
                    account = entity.Account,
                    account_digit = entity.AccountDigit,
                    transferor_account = entity.TransferorAccount,
                    local_to_pay = entity.LocalToPay,
                    bank_id = entity.BankId
                };

                entity.Id = conn.ExecuteScalar<int>("spi_platform_billet_bank_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public PlatformBilletBankData Update(PlatformBilletBankData entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    agency = entity.Agency,
                    account = entity.Account,
                    account_digit = entity.AccountDigit,
                    transferor_account = entity.TransferorAccount,
                    local_to_pay = entity.LocalToPay,
                    bank_id = entity.BankId
                };

                conn.Execute("spu_platform_billet_bank_data", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<PlatformBilletBankData> FindAll()
        {
            IEnumerable<PlatformBilletBankData> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<PlatformBilletBankData>("sps_platform_billet_bank_data");
            }
            return entities;
        }

        public PlatformBilletBankData FindById(int id)
        {
            PlatformBilletBankData entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.Query<PlatformBilletBankData, Bank, PlatformBilletBankData>("sps_platform_billet_bank_data_by_pk", param: new { id }, map: (platformBilletBankData, bank)=>{
                       platformBilletBankData.Bank = bank;
                       return platformBilletBankData;
                    }, 
                    splitOn: "bank.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            return entity;
        }
		public List<PlatformBilletBankData> FindByBankId (Int32 BankId){throw new NotImplementedException();}
    }
}