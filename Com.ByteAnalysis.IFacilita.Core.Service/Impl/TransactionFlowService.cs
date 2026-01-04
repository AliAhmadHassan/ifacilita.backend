using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionFlowService : ITransactionFlowService
    {
        Repository.ITransactionFlowRepository repository;

        public TransactionFlowService(Repository.ITransactionFlowRepository repository)
        {
            this.repository = repository;
        }
        public void Delete(int idTransaction, int idPlatformSubWorkflow)
        {
            this.repository.Delete(idTransaction, idPlatformSubWorkflow);
        }

        public IEnumerable<TransactionFlow> FindByIdTransaction(int idTransaction)
        {
            return this.repository.findByIdTransaction(idTransaction);
        }

        public TransactionFlow Update(TransactionFlow entity)
        {
            return this.repository.Update(entity);
        }
    }
}
