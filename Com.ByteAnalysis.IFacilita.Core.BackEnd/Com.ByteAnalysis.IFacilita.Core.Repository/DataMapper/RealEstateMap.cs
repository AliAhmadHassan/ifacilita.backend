using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class RealEstateMap: EntityMap<RealEstate>
    {
        internal RealEstateMap()
        {
            Map(u => u.RegisteredNumber).ToColumn("registered_number");
            Map(u => u.CorporateName).ToColumn("corporate_name");
        }
    }
}
