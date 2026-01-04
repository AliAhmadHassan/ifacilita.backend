using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PatrimonyMap: EntityMap<Patrimony>
    {
        internal PatrimonyMap()
        {
            Map(u => u.MunicipalRegistration).ToColumn("municipal_registration");
            Map(u => u.Registration).ToColumn("registration");
            Map(u => u.Bedrooms).ToColumn("bedrooms");
            Map(u => u.MaidsRoom).ToColumn("maids_room");
            Map(u => u.NumberOfCarSpaces).ToColumn("number_of_car_spaces");
            Map(u => u.ForeiroProperty).ToColumn("foreiro_property");
            Map(u => u.BathroomsExceptForMaids).ToColumn("bathrooms_except_for_maids");
            Map(u => u.MaidBathroom).ToColumn("maid_bathroom");
            Map(u => u.Balcony).ToColumn("balcony");
            Map(u => u.FloorPosition).ToColumn("floor_position");
            Map(u => u.IdAddress).ToColumn("idaddress");
        }
    }
}
