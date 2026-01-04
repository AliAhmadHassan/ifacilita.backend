using Com.ByteAnalysis.IFacilita.Draft.Model;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Draft.Service
{
    public interface ITransactionService
    {
        List<Transaction> Get();

        Transaction Get(string id);

        Transaction Create(Transaction transaction);

        void Update(string id, Transaction transactionIn);

        void Remove(Transaction transactionIn);

        void Remove(string id);
        string GetPDF(string id);
    }
}
