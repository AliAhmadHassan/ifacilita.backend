using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionPaymentFormRepository: ICrudRepository<Entity.TransactionPaymentForm, int>
    {
        IEnumerable<Entity.TransactionPaymentForm> FindByIdtransaction(int idtransaction);
    }
}
