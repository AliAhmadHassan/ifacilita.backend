using Com.ByteAnalysis.IFacilita.Chat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Chat.Repository
{
    public interface ITransactionRepository
    {
        List<Transaction> Get();

        Transaction Get(string id);

        Transaction Create(Transaction transaction);

        void Update(string id, Transaction transactionIn);

        void Remove(Transaction transactionIn);

        void Remove(string id);
    }
}
