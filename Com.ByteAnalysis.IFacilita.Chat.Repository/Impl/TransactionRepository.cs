using Com.ByteAnalysis.IFacilita.Chat.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Chat.Repository.Impl
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _transactions = database.GetCollection<Transaction>("transactions");
        }

        public Transaction Create(Transaction transaction)
        {
            _transactions.InsertOne(transaction);
            return transaction;
        }

        public List<Transaction> Get() => _transactions.Find<Transaction>(transaction => true).ToList();

        public Transaction Get(string id) => _transactions.Find<Transaction>(transaction => transaction.Id.Equals(id)).FirstOrDefault();

        public void Remove(Transaction transactionIn) => _transactions.DeleteOne(transaction => transaction.Id.Equals(transactionIn.Id));

        public void Remove(string id) => _transactions.DeleteOne(transaction => transaction.Id.Equals(id));

        public void Update(string id, Transaction transactionIn) => _transactions.ReplaceOne(transaction => transaction.Id == id, transactionIn);
    }
}
