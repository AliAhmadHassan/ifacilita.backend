using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionRepository : ITransactionRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_transaction", new { id });
            }
        }

        public Transaction Insert(Transaction entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    date = entity.Date,
                    iduser = entity.IdUser,
                    iduser_seller = entity.Seller,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    idregistry = entity.IdRegistry
                };

                entity.Id = conn.ExecuteScalar<int>("spi_transaction", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Transaction Update(Transaction entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    id = entity.Id,
                    date = entity.Date,
                    iduser = entity.IdUser,
                    iduser_seller = entity.Seller,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    idregistry = entity.IdRegistry
                };

                conn.Execute("spu_transaction", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<Transaction> FindAll()
        {
            IEnumerable<Transaction> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<Transaction>("sps_transaction");
            }
            return entities;
        }

        public Transaction FindById(int id)
        {
            Transaction entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<Transaction>("sps_transaction_by_pk", new { id });
            }
            return entity;
        }
		public List<Transaction> FindByIdUser (Int32 IdUser){throw new NotImplementedException();}
    }
}