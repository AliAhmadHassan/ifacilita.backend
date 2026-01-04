using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserBankDataMap: EntityMap<UserBankData>
    {
        internal UserBankDataMap()
        {
            Map(u => u.Id).ToColumn("id");
            Map(u => u.Agency).ToColumn("agency");
            Map(u => u.Account).ToColumn("account");
            Map(u => u.AccountDigit).ToColumn("account_digit");
        }
    }
}
