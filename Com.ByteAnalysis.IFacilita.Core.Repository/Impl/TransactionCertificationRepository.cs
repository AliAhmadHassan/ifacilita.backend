using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.Impl
{
    public class TransactionCertificationRepository : ITransactionCertificationRepository
    {
        IDatabaseSettings databaseSettings;

        public TransactionCertificationRepository(IDatabaseSettings databaseSettings)
        {
            this.databaseSettings = databaseSettings;
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Execute("spd_transaction_certification", new { id }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<TransactionCertification> FindAll()
        {
            throw new NotImplementedException();
        }

        public TransactionCertification FindById(int id)
        {
            TransactionCertification transactionCertification = null;
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                transactionCertification = conn.QueryFirstOrDefault<Entity.TransactionCertification>("sps_transaction_certification_by_pk", new { id }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionCertification;
        }

        public IEnumerable<TransactionCertification> FindByIdtransaction(int idtransaction)
        {
            IEnumerable<TransactionCertification> transactionCertifications = null;
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                transactionCertifications = conn.Query<Entity.TransactionCertification>("sps_transaction_certification_by_idtransaction", new { idtransaction }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return transactionCertifications;
        }

        public TransactionCertification Insert(TransactionCertification entity)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                entity.Id = conn.ExecuteScalar<int>("spi_transaction_certification", new {
                    idtransaction = entity.Idtransaction,
                    certificate_name = entity.CertificateName,
                    certificate_path = entity.CertificatePath,
                    certificate_filename = entity.CertificateFilename,
                    received = entity.Received,
                    buyer_seen = entity.BuyerSeen,
                    buyer_accept = entity.BuyerAccept,
                    buyer_refuse = entity.BuyerRefuse,
                    buyer_response = entity.BuyerResponse,
                    buyer_we_solve = entity.BuyerWeSolve,
                    buyer_why_rejected = entity.BuyerWhyRejected,
                    seller_seen = entity.SellerSeen,
                    seller_accept = entity.SellerAccept,
                    seller_refuse = entity.SellerRefuse,
                    seller_response = entity.SellerResponse,
                    seller_we_solve = entity.SellerWeSolve,
                    seller_why_rejected = entity.SellerWhyRejected,
                    expiration_date = entity.ExpirationDate,
                    receipt_forecast = entity.ReceiptForecast,
                    value = entity.Value,
                    ecartorio_id = entity.EcartorioId,
                    notary= entity.Notary
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public TransactionCertification Update(TransactionCertification entity)
        {
            using (SqlConnection conn = new SqlConnection(this.databaseSettings.ConnectionString))
            {
                conn.Execute("spu_transaction_certification", new
                {
                    id = entity.Id,
                    idtransaction = entity.Idtransaction,
                    certificate_name = entity.CertificateName,
                    certificate_path = entity.CertificatePath,
                    certificate_filename = entity.CertificateFilename,
                    received = entity.Received,
                    buyer_seen = entity.BuyerSeen,
                    buyer_accept = entity.BuyerAccept,
                    buyer_refuse = entity.BuyerRefuse,
                    buyer_response = entity.BuyerResponse,
                    buyer_we_solve = entity.BuyerWeSolve,
                    buyer_why_rejected = entity.BuyerWhyRejected,
                    seller_seen = entity.SellerSeen,
                    seller_accept = entity.SellerAccept,
                    seller_refuse = entity.SellerRefuse,
                    seller_response = entity.SellerResponse,
                    seller_we_solve = entity.SellerWeSolve,
                    seller_why_rejected = entity.SellerWhyRejected,
                    expiration_date = entity.ExpirationDate,
                    receipt_forecast = entity.ReceiptForecast,
                    value = entity.Value,
                    ecartorio_id = entity.EcartorioId,
                    notary = entity.Notary
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }
    }
}
