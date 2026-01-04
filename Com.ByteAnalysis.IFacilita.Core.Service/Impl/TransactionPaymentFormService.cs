using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionPaymentFormService : ITransactionPaymentFormService
    {
        Repository.ITransactionPaymentFormRepository repository;
        Service.ITransactionService transactionService;
        Service.IPushNotificationService pushNotificationService;
        Repository.ITransactionFlowRepository transactionFlowRepository;
        public TransactionPaymentFormService(Repository.ITransactionPaymentFormRepository repository,
            Service.ITransactionService transactionService,
            Service.IPushNotificationService pushNotificationService,
            Repository.ITransactionFlowRepository transactionFlowRepository)
        {
            this.repository = repository;
            this.transactionService = transactionService;
            this.pushNotificationService = pushNotificationService;
            this.transactionFlowRepository = transactionFlowRepository;
        }
        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<TransactionPaymentForm> FindAll()
        {
            return this.repository.FindAll();
        }

        public TransactionPaymentForm FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public IEnumerable<TransactionPaymentForm> FindByIdtransaction(int idtransaction)
        {
            return this.repository.FindByIdtransaction(idtransaction);
        }

        public TransactionPaymentForm Insert(TransactionPaymentForm entity)
        {
            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.IdTransaction).Where(c => c.IdplatformSubWorkflow.Equals(1044)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlow.Status = 2;
                this.transactionFlowRepository.Update(transactionFlow);
            }
            Transaction transaction = this.transactionService.FindById(entity.IdTransaction);

            this.pushNotificationService.SendMessage(transaction.Seller.Value, "iFacilita - Forma de pagamento", $"Forma de pagamento foi cadastrado.", "", "https://ifacilita/logged-promise/upload-documents");

            var currentPaymentForm = repository.FindByIdtransaction(entity.IdTransaction).FirstOrDefault();
            if (currentPaymentForm == null)
            {
                return repository.Insert(entity);
            }
            else
            {
                
                entity.IdTransaction = currentPaymentForm.IdTransaction;
                entity.Plain = currentPaymentForm.Plain;
                entity.Value = currentPaymentForm.Value;
                entity.Id = currentPaymentForm.Id;

                return repository.Update(entity);
            }
                

        }

        public TransactionPaymentForm Update(TransactionPaymentForm entity)
        {
            Transaction transaction = this.transactionService.FindById(entity.IdTransaction);

            this.pushNotificationService.SendMessage(transaction.Seller.Value, "iFacilita - Forma de pagamento", $"Forma de pagamento foi alterado.", "", "https://ifacilita/logged-promise/upload-documents");
            return this.repository.Update(entity);
        }
    }
}
