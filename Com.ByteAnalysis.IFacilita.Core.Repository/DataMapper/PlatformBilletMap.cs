using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PlatformBilletMap: EntityMap<PlatformBillet>
    {
        internal PlatformBilletMap()
        {
            Map(u => u.OurNumber).ToColumn("platform_billet.our_number");
            Map(u => u.Value).ToColumn("platform_billet.value");
            Map(u => u.DueDate).ToColumn("platform_billet.due_date");
            Map(u => u.Created).ToColumn("platform_billet.created");
            Map(u => u.PayDay).ToColumn("platform_billet.pay_day");
            Map(u => u.Paid).ToColumn("platform_billet.paid");
            Map(u => u.IdPlatformBilletBankData).ToColumn("platform_billet.idplatform_billet_bank_data");
            Map(u => u.IdUser).ToColumn("platform_billet.iduser");
        }
    }
}
