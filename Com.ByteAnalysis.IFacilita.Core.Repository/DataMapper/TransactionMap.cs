using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionMap: EntityMap<Transaction>
    {
        internal TransactionMap()
        {
            Map(u => u.Id).ToColumn("transaction.id");
            Map(u => u.Date).ToColumn("transaction.date");
            Map(u => u.IdUser).ToColumn("transaction.iduser");
            Map(u => u.Seller).ToColumn("transaction.iduser_seller");
            Map(u => u.PatrimonyMunicipalRegistration).ToColumn("transaction.patrimony_municipal_registration");
            Map(u => u.IdPatrimonyAcquirerType).ToColumn("transaction.idpatrimony_acquirer_type");
            Map(u => u.IdPatrimonyTransmitterType).ToColumn("transaction.idpatrimony_transmitter_type");
            Map(u => u.IdRegistry).ToColumn("transaction.idregistry");
            Map(u => u.Signal).ToColumn("transaction.signal");
            Map(u => u.PromiseVoucher).ToColumn("transaction.promise_voucher");
            Map(u => u.CertificateVoucher).ToColumn("transaction.certificate_voucher");
            Map(u => u.ItbiVoucher).ToColumn("transaction.itbi_voucher");
            Map(u => u.IdDraft).ToColumn("transaction.iddraft");
            Map(u => u.KeyDelivery).ToColumn("transaction.key_delivery");
            Map(u => u.RegistryToken).ToColumn("transaction.registry_token");
            Map(u => u.ContractToken).ToColumn("transaction.contract_token");
            Map(u => u.IdPatrimony).ToColumn("transaction.idpatrimony");
        }
    }
}
