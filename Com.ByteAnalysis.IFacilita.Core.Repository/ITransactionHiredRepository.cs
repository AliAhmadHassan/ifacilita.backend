using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionHiredRepository : ICrudRepository<Entity.TransactionHired, int>
    {
        IEnumerable<Entity.TransactionHired> FindByIdTransaction(int idTransaction);
    }
}
