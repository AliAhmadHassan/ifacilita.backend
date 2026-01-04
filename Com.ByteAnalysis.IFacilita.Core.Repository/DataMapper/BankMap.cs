using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class BankMap: EntityMap<Bank>
    {
        internal BankMap()
        {
            Map(u => u.Id).ToColumn("bank.id");
            Map(u => u.CorporateName).ToColumn("bank.corporate_name");
        }
    }
}
