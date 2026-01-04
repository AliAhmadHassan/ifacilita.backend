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
        private readonly IAddressRepository _addressRepository;

        public TransactionRepository(IDatabaseSettings databaseSettings, IAddressRepository addressRepository)
        {
            this.databaseSettings = databaseSettings;
            _addressRepository = addressRepository;
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
                var values = new
                {
                    date = entity.Date,
                    iduser = entity.IdUser,
                    iduser_seller = entity.Seller,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    idregistry = entity.IdRegistry,
                    signal = entity.Signal,
                    promise_voucher = entity.PromiseVoucher,
                    certificate_voucher = entity.CertificateVoucher,
                    itbi_voucher = entity.ItbiVoucher,
                    iddraft = entity.IdDraft,
                    key_delivery = entity.KeyDelivery,
                    registry_token = entity.RegistryToken,
                    contract_token = entity.ContractToken,
                    idpatrimony = entity.IdPatrimony
                };

                entity.Id = conn.ExecuteScalar<int>("spi_transaction", values, commandType: System.Data.CommandType.StoredProcedure);
            }
            return entity;
        }

        public Transaction Update(Transaction entity)
        {
            using (SqlConnection conn = new SqlConnection(databaseSettings.ConnectionString))
            {
                var values = new
                {
                    id = entity.Id,
                    date = entity.Date,
                    iduser = entity.IdUser,
                    iduser_seller = entity.Seller,
                    patrimony_municipal_registration = entity.PatrimonyMunicipalRegistration,
                    idpatrimony_acquirer_type = entity.IdPatrimonyAcquirerType,
                    idpatrimony_transmitter_type = entity.IdPatrimonyTransmitterType,
                    idregistry = entity.IdRegistry,
                    signal = entity.Signal,
                    promise_voucher = entity.PromiseVoucher,
                    certificate_voucher = entity.CertificateVoucher,
                    itbi_voucher = entity.ItbiVoucher,
                    iddraft = entity.IdDraft,
                    key_delivery = entity.KeyDelivery,
                    registry_token = entity.RegistryToken,
                    contract_token = entity.ContractToken,
                    idpatrimony = entity.IdPatrimony
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
                entity = conn.Query<Transaction, User, User, Patrimony, PatrimonyAcquirerType, PatrimonyTransmitterType, Registry, Transaction>("sps_transaction_by_pk", param: new { id },
                    map: (transaction, user, user_seller, patrimony, patrimonyAcquirerType, patrimonyTransmitterType, registry) =>
                    {
                        transaction.User = user;
                        transaction.User_Seller = user_seller;
                        transaction.Patrimony = patrimony;
                        transaction.PatrimonyAcquirerType = patrimonyAcquirerType;
                        transaction.PatrimonyTransmitterType = patrimonyTransmitterType;
                        transaction.Registry = registry;
                        return transaction;
                    },
                    splitOn: "user.id, user.id, patrimony.municipal_registration, patrimony_acquirer_type.id, patrimony_transmitter_type.id, registry.id",
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            if(entity != null && entity.Patrimony != null)
            {
                entity.Patrimony.Address = _addressRepository.FindById(entity.Patrimony.IdAddress.Value);
            }

            return entity;
        }

        public List<Transaction> FindByIdUser(Int32 IdUser) { throw new NotImplementedException(); }
    }
}