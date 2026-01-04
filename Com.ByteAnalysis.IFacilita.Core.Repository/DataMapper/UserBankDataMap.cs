using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class UserBankDataMap: EntityMap<UserBankData>
    {
        internal UserBankDataMap()
        {
            Map(u => u.Id).ToColumn("user_bank_data.id");
            Map(u => u.Agency).ToColumn("user_bank_data.agency");
            Map(u => u.Account).ToColumn("user_bank_data.account");
            Map(u => u.AccountDigit).ToColumn("user_bank_data.account_digit");
        }
    }
}
