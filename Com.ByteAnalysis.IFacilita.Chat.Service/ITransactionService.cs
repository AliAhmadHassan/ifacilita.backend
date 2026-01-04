using Com.ByteAnalysis.IFacilita.Chat.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Chat.Service
{
    public interface ITransactionService
    {
        List<Transaction> Get();

        Transaction Get(string id);

        Transaction Create(Transaction transaction);

        void Update(string id, Transaction transactionIn);

        void Remove(string id);
    }
}
