using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class RealEstateMap: EntityMap<RealEstate>
    {
        internal RealEstateMap()
        {
            Map(u => u.RegisteredNumber).ToColumn("real_estate.registered_number");
            Map(u => u.CorporateName).ToColumn("real_estate.corporate_name");
        }
    }
}
