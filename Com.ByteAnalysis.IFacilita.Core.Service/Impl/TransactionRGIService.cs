using Com.ByteAnalysis.IFacilita.Core.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionRGIService : ITransactionRGIService
    {
        Repository.ITransactionRGIRepository repository;
        Common.IHttpClientFW httpClientFW;
        Entity.IApplicationSettings applicationSettings;
        private readonly ILogger<TransactionRGIService> _logger;
        Service.ITransactionService transactionService;
        Common.IS3 s3;
        Repository.ITransactionFlowRepository transactionFlowRepository;

        public TransactionRGIService(Repository.ITransactionRGIRepository repository,
            Entity.IApplicationSettings applicationSettings,
            Service.ITransactionService transactionService,
            Repository.ITransactionFlowRepository transactionFlowRepository,
            ILogger<TransactionRGIService> _logger)
        {
            this.applicationSettings = applicationSettings;
            this.repository = repository;
            httpClientFW = new Common.Impl.HttpClientFW(this.applicationSettings.URLRGIApi);
            this._logger = _logger;
            this.transactionService = transactionService;
            this.s3 = new Common.Impl.S3();
            this.transactionFlowRepository = transactionFlowRepository;
        }

        public void CallRPA(TransactionRGI entity)
        {
            Entity.Transaction transaction = this.transactionService.FindById(entity.IdTransaction);
            string base64Document = s3.GetFile(transaction.ContractToken.Replace(".pdf", "").Replace("https://ifacilita.s3.us-east-2.amazonaws.com/", ""));

            var returned = httpClientFW.Post<object, object>(new string[] { }, new
            {
                IdTransaction = entity.IdTransaction,
                UrlCallback = "https://ifacilita.com:5000/api/TransactionRGI/{0}/callback",
                ShipmentData = new
                {
                    TitleDate = entity.TitleDate,
                    Book = entity.Book,
                    Sheet = entity.Sheet,
                    Sellers = new []{
                        new
                        {
                            TypeOfDocument = 0,
                            DocumentNumber = transaction.User_Seller.SocialSecurityNumber,
                            Name = $"{transaction.User_Seller.Name} {transaction.User_Seller.LastName}".Trim()
                        }
                    },
                    Buyers = new[]{
                        new
                        {
                            TypeOfDocument = 0,
                            DocumentNumber = transaction.User.SocialSecurityNumber,
                            Name = $"{transaction.User.Name} {transaction.User.LastName}".Trim()
                        }
                    },
                    ReceivingNotary = new
                    {
                        City = entity.NotaryCity,
                        NotaryNumber = entity.NotaryNumber
                    },
                    Base64Document = base64Document
                }
            });
            var id = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JObject)returned.Value)["id"]).Value;
            entity.RpaKey = id.ToString();
            repository.Update(entity);

            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.IdTransaction).Where(c => c.IdplatformSubWorkflow.Equals(1047)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;
            this.transactionFlowRepository.Update(transactionFlow);
            //1047
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<TransactionRGI> FindAll()
        {
            return this.repository.FindAll();
        }

        public TransactionRGI FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public TransactionRGI Insert(TransactionRGI entity)
        {
            return this.repository.Insert(entity);
        }

        public TransactionRGI Update(TransactionRGI entity)
        {
            return this.repository.Update(entity);
        }

        public void Completed(int idTransaction)
        {
            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(idTransaction).Where(c => c.IdplatformSubWorkflow.Equals(1048)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;
            this.transactionFlowRepository.Update(transactionFlow);

        }
    }
}
