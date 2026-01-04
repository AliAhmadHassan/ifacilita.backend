using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionDocumentRepository: ICrudRepository<TransactionDocument, int>
    {
		List<TransactionDocument> FindByIdTransaction (Int32 IdTransaction);
    }
}
