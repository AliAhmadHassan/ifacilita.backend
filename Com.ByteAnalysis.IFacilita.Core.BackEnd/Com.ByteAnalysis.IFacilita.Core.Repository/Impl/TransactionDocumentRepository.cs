using Dapper;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionDocumentRepository : ITransactionDocumentRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionDocumentRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spd_transaction_document", new { id });
            }
        }

        public TransactionDocument Insert(TransactionDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    idtransaction = entity.IdTransaction,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spi_transaction_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public TransactionDocument Update(TransactionDocument entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new {
                    idtransaction = entity.IdTransaction,
                    iddocument = entity.IdDocument
                };

                conn.Execute("spu_transaction_document", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public IEnumerable<TransactionDocument> FindAll()
        {
            IEnumerable<TransactionDocument> entities = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entities = conn.Query<TransactionDocument>("sps_transaction_document");
            }
            return entities;
        }

        public TransactionDocument FindById(int id)
        {
            TransactionDocument entity = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                entity = conn.QueryFirstOrDefault<TransactionDocument>("sps_transaction_document_by_pk", new { id });
            }
            return entity;
        }
		public List<TransactionDocument> FindByIdTransaction (Int32 IdTransaction){throw new NotImplementedException();}
    }
}