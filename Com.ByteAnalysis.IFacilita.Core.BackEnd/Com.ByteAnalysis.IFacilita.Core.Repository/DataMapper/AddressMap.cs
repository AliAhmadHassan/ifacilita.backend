using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class AddressMap: EntityMap<Address>
    {
        internal AddressMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Street).ToColumn("street");
            Map(u => u.Number).ToColumn("number");
            Map(u => u.Complement).ToColumn("complement");
            Map(u => u.District).ToColumn("district");
            Map(u => u.ZipCode).ToColumn("zip_code");
            Map(u => u.CitySig).ToColumn("city_sig");
        }
    }
}
