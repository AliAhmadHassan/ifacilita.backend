using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class AddressMap: EntityMap<Address>
    {
        internal AddressMap()
        {
            Map(u => u.Id).ToColumn("address.id");
            Map(u => u.Street).ToColumn("address.street");
            Map(u => u.Number).ToColumn("address.number");
            Map(u => u.Complement).ToColumn("address.complement");
            Map(u => u.District).ToColumn("address.district");
            Map(u => u.ZipCode).ToColumn("address.zip_code");
            Map(u => u.CitySig).ToColumn("address.city_sig");
        }
    }
}
