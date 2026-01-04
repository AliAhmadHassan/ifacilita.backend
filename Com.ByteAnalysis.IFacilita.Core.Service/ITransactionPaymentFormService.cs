using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionPaymentFormService: ICrudService<Entity.TransactionPaymentForm, int>
    {
        IEnumerable<Entity.TransactionPaymentForm> FindByIdtransaction(int idtransaction);
    }
}
