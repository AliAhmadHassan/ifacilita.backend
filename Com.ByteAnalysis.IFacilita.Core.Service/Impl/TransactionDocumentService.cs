using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionDocumentService : ITransactionDocumentService
    {
        Repository.ITransactionDocumentRepository repository;
        Service.ITransactionService transactionService;
        Service.IPushNotificationService pushNotificationService;

        public TransactionDocumentService(ITransactionDocumentRepository repository,
            Service.ITransactionService transactionService,
            Service.IPushNotificationService pushNotificationService)
        {
            this.repository = repository;
            this.transactionService = transactionService;
            this.pushNotificationService = pushNotificationService;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<TransactionDocument> FindAll()
        {
            return repository.FindAll();
        }

        public TransactionDocument FindById(int id)
        {
            return repository.FindById(id);
        }

        public TransactionDocument Insert(TransactionDocument entity)
        {


            return repository.Insert(entity);
        }

        public TransactionDocument Update(TransactionDocument entity)
        {
            return repository.Update(entity);
        }
    }
}
