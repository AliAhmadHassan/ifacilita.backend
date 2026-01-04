using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionMap: EntityMap<Transaction>
    {
        internal TransactionMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Date).ToColumn("date");
            Map(u => u.IdUser).ToColumn("iduser");
            Map(u => u.Seller).ToColumn("iduser_seller");
            Map(u => u.PatrimonyMunicipalRegistration).ToColumn("patrimony_municipal_registration");
            Map(u => u.IdPatrimonyAcquirerType).ToColumn("idpatrimony_acquirer_type");
            Map(u => u.IdPatrimonyTransmitterType).ToColumn("idpatrimony_transmitter_type");
            Map(u => u.IdRegistry).ToColumn("idregistry");
        }
    }
}
