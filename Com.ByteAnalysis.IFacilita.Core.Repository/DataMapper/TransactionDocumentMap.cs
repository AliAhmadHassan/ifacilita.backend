using Dapper.FluentMap.Mapping;
using Com.ByteAnalysis.IFacilita.Core.Entity;

namespace Com.ByteAnalysis.IFacilita.Core.Repository.DataMapper
{
    public class TransactionDocumentMap: EntityMap<TransactionDocument>
    {
        internal TransactionDocumentMap()
        {
            Map(u => u.IdTransaction).ToColumn("transaction_document.idtransaction");
            Map(u => u.IdDocument).ToColumn("transaction_document.iddocument");
        }
    }
}
