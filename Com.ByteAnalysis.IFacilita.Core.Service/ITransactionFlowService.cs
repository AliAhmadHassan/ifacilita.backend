using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionFlowService
    {
        void Delete(int idTransaction, int idPlatformSubWorkflow);

        IEnumerable<Entity.TransactionFlow> FindByIdTransaction(int idTransaction);

        Entity.TransactionFlow Update(Entity.TransactionFlow entity); 
    }
}
