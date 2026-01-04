using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class PlatformBilletBankDataMap: EntityMap<PlatformBilletBankData>
    {
        internal PlatformBilletBankDataMap()
        {
            Map(u => u.Id).ToColumn("platform_billet_bank_data.id");
            Map(u => u.Agency).ToColumn("platform_billet_bank_data.agency");
            Map(u => u.Account).ToColumn("platform_billet_bank_data.account");
            Map(u => u.AccountDigit).ToColumn("platform_billet_bank_data.account_digit");
            Map(u => u.TransferorAccount).ToColumn("platform_billet_bank_data.transferor_account");
            Map(u => u.LocalToPay).ToColumn("platform_billet_bank_data.local_to_pay");
            Map(u => u.BankId).ToColumn("platform_billet_bank_data.bank_id");
        }
    }
}
