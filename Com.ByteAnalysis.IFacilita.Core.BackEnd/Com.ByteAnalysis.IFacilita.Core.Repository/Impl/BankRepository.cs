using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class BankRepository : IBankRepository
    {
        IDatabaseSettings databaseSettings;

        public BankRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_bank", new { id });
            }
        }

        public Bank Insert(Bank entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    corporate_name = entity.CorporateName
                };

                conn.Execute("spi_bank", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Bank Update(Bank entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    corporate_name = entity.CorporateName
                };

                conn.Execute("spu_bank", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Bank> FindAll()
        {
            IEnumerable<Bank> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Bank>("sps_bank");
            }
            return entities;
        }

        public Bank FindById(int id)
        {
            Bank entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Bank>("sps_bank_by_pk", new { id });
            }
            return entity;
        }
    }
}