using Com.ByteAnalysis.IFacilita.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionHiredRepository : ITransactionHiredRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionHiredRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransactionHired> FindAll()
        {
            throw new NotImplementedException();
        }

        public TransactionHired FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransactionHired> FindByIdTransaction(int idTransaction)
        {
            IEnumerable<TransactionHired> transactionHireds = null;
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                transactionHireds = conn.Query<TransactionHired, Transaction, PlatformWorkflow, TransactionHired>("sps_transaction_hired_by_idtransaction", param: new { idtransaction = idTransaction }, 
                    map: (transactionHired, transaction, platformWorkflow) => {
                        transactionHired.Transaction = transaction;
                        transactionHired.PlatformWorkflow = platformWorkflow;
                        return transactionHired;
                    },
                    splitOn: "split, split",
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            return transactionHireds;
        }

        public TransactionHired Insert(TransactionHired entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spi_transaction_hired", new { 
                    idtransaction = entity.IdTransaction,
                    idplatform_workflow = entity.IdPlatformWorkflow
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public TransactionHired Update(TransactionHired entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                conn.Execute("spu_transaction_hired", new
                {
                    idtransaction = entity.IdTransaction,
                    idplatform_workflow = entity.IdPlatformWorkflow,
                    completed = entity.Completed
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
