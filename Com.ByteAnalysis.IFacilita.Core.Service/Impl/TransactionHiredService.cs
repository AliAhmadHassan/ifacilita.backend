using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionHiredService : ITransactionHiredService
    {
        Repository.ITransactionHiredRepository repository;

        public TransactionHiredService(Repository.ITransactionHiredRepository repository)
        {
            this.repository = repository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<TransactionHired> FindAll()
        {
            return this.repository.FindAll();
        }

        public TransactionHired FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public IEnumerable<TransactionHired> FindByIdTransaction(int idTransaction)
        {
            return this.repository.FindByIdTransaction(idTransaction);
        }

        public TransactionHired Insert(TransactionHired entity)
        {
            return this.repository.Insert(entity);
        }

        public TransactionHired Update(TransactionHired entity)
        {
            return this.repository.Update(entity);
        }
    }
}
