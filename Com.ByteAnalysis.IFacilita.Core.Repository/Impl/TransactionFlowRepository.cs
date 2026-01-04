using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.SqlClient;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionFlowRepository : ITransactionFlowRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionFlowRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int idTransaction, int idPlatformSubWorkflow)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Open();
                conn.Execute("spd_transaction_flow", new { idTransaction, idplatform_sub_workflow = idPlatformSubWorkflow }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<TransactionFlow> findByIdTransaction(int idTransaction)
        {
            IEnumerable<TransactionFlow> transactionFlows = null;
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Open();
                transactionFlows = conn.Query<Entity.TransactionFlow>("sps_transaction_flow_by_idtransaction", new { idTransaction }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionFlows;
        }

        public TransactionFlow Update(TransactionFlow entity)
        {
            TransactionFlow transactionFlows = null;
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Open();
                conn.Execute("spu_transaction_flow", new { idTransaction = entity.Idtransaction,
                    idplatform_sub_workflow = entity.IdplatformSubWorkflow,
                    status = entity.Status,
                    status_changed = entity.StatusChanged
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionFlows;
        }
    }
}
