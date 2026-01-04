using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionFlowRepository
    {
        void Delete(int idTransaction, int idPlatformSubWorkflow);
        IEnumerable<Entity.TransactionFlow> findByIdTransaction(int idTransaction);

        Entity.TransactionFlow Update(Entity.TransactionFlow entity);
    }
}
