using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionPaymentFormRepository : ITransactionPaymentFormRepository
    {
        Entity.IDatabaseSettings databaseSettings;

        public TransactionPaymentFormRepository(Entity.IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Execute("spd_transaction_payment_form", new
                {
                    id = id
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<TransactionPaymentForm> FindAll()
        {
            throw new NotImplementedException();
        }

        public TransactionPaymentForm FindById(int id)
        {
            TransactionPaymentForm transactionPaymentForm = null;

            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                transactionPaymentForm = conn.QueryFirstOrDefault<TransactionPaymentForm>("sps_transaction_payment_form_by_pk", new { id }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionPaymentForm;
        }

        public IEnumerable<TransactionPaymentForm> FindByIdtransaction(int idtransaction)
        {
            IEnumerable<TransactionPaymentForm> transactionPaymentForms = null;
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                transactionPaymentForms = conn.Query<TransactionPaymentForm>("sps_transaction_payment_form_by_idtransaction", new { idtransaction }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionPaymentForms;
        }

        public TransactionPaymentForm Insert(TransactionPaymentForm entity)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                entity.Id = conn.ExecuteScalar<int>("spi_transaction_payment_form", new
                {
                    idtransaction = entity.IdTransaction,
                    plain = entity.Plain,
                    value = entity.Value
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public TransactionPaymentForm Update(TransactionPaymentForm entity)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.ExecuteScalar("spu_transaction_payment_form", new
                {
                    id = entity.Id,
                    idtransaction = entity.IdTransaction,
                    plain = entity.Plain,
                    value = entity.Value
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
