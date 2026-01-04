using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PlatformBilletMap: EntityMap<PlatformBillet>
    {
        internal PlatformBilletMap()
        {
            Map(u => u.OurNumber).ToColumn("our_number");
            Map(u => u.Value).ToColumn("value");
            Map(u => u.DueDate).ToColumn("due_date");
            Map(u => u.Created).ToColumn("created");
            Map(u => u.PayDay).ToColumn("pay_day");
            Map(u => u.Paid).ToColumn("paid");
            Map(u => u.IdPlatformBilletBankData).ToColumn("idplatform_billet_bank_data");
            Map(u => u.IdUser).ToColumn("iduser");
        }
    }
}
