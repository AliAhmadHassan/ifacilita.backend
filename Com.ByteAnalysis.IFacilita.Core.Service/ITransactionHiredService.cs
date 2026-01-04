using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionHiredService : ICrudService<Entity.TransactionHired, int>
    {
        IEnumerable<TransactionHired> FindByIdTransaction(int idTransaction);
    }
}
