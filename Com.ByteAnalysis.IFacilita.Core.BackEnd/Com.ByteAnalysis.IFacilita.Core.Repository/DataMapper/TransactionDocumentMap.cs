using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionDocumentMap: EntityMap<TransactionDocument>
    {
        internal TransactionDocumentMap()
        {
            Map(u => u.IdTransaction).ToColumn("idtransaction");
            Map(u => u.IdDocument).ToColumn("iddocument");
        }
    }
}
