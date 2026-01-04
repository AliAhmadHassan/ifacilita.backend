using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionRGIRepository : ITransactionRGIRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionRGIRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransactionRGI> FindAll()
        {
            throw new NotImplementedException();
        }

        public TransactionRGI FindById(int id)
        {
            TransactionRGI transactionRGI = null;

            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                transactionRGI = conn.Query<TransactionRGI>("sps_transaction_rgi_by_idtransaction", param: new { idtransaction = id },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            return transactionRGI;
        }

        public TransactionRGI Insert(TransactionRGI entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spi_transaction_rgi", new
                {
                    idtransaction = entity.IdTransaction,
                    title_data = entity.TitleDate,
                    book = entity.Book,
                    sheet = entity.Sheet,
                    notary_city = entity.NotaryCity,
                    notary_number = entity.NotaryNumber,
                    rpa_key = entity.RpaKey
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public TransactionRGI Update(TransactionRGI entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spu_transaction_rgi", new
                {
                    idtransaction = entity.IdTransaction,
                    title_data = entity.TitleDate,
                    book = entity.Book,
                    sheet = entity.Sheet,
                    notary_city = entity.NotaryCity,
                    notary_number = entity.NotaryNumber,
                    rpa_key = entity.RpaKey
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
