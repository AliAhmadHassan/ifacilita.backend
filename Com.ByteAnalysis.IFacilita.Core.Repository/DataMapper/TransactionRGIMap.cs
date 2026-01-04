using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionRGIMap : EntityMap<TransactionRGI>
    {
        internal TransactionRGIMap()
        {
            Map(u => u.IdTransaction).ToColumn("idtransaction");
            Map(u => u.TitleDate).ToColumn("title_data");
            Map(u => u.Book).ToColumn("book");
            Map(u => u.Sheet).ToColumn("sheet");
            Map(u => u.NotaryCity).ToColumn("notary_city");
            Map(u => u.NotaryNumber).ToColumn("notary_number");
            Map(u => u.RpaKey).ToColumn("rpa_key");
        }
    }
}
