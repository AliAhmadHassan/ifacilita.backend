using Com.ByteAnalysis.IFacilita.Chat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Chat.Service.Impl
{
    public class TransactionService: ITransactionService
    {
        Repository.ITransactionRepository repository;
        public TransactionService(Repository.ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public Transaction Create(Transaction transaction)
        {
            transaction.Id = transaction.Id.PadLeft(24, '0');
            return this.repository.Create(transaction);
        }

        public List<Transaction> Get()
        {
            return this.repository.Get();
        }

        public Transaction Get(string id)
        {
            return this.repository.Get(id.PadLeft(24, '0'));
        }

        public void Remove(string id)
        {
            this.repository.Remove(id.PadLeft(24, '0'));
        }

        public void Update(string id, Transaction transactionIn)
        {
            transactionIn.Id = transactionIn.Id.PadLeft(24, '0');
            this.repository.Update(id.PadLeft(24, '0'), transactionIn);
        }
    }
}
