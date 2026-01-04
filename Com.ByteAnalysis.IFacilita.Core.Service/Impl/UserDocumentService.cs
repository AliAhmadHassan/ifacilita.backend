using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserDocumentService : IUserDocumentService
    {
        Repository.IUserDocumentRepository repository;
        Entity.IPathSettings pathSettings;
        Service.ITransactionService transactionService;
        Service.IPushNotificationService pushNotificationService;
        Service.IUserService userService;
        Service.IHistoricService historicService;
        Common.IS3 s3 = new Common.Impl.S3();

        public UserDocumentService(IUserDocumentRepository repository,
            Entity.IPathSettings pathSettings,
            Service.ITransactionService transactionService,
            Service.IPushNotificationService pushNotificationService,
            Service.IUserService userService,
            Service.IHistoricService historicService)
        {
            this.repository = repository;
            this.pathSettings = pathSettings;
            this.transactionService = transactionService;
            this.pushNotificationService = pushNotificationService;
            this.userService = userService;
            this.historicService = historicService;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<UserDocument> FindAll()
        {
            return repository.FindAll();
        }

        public UserDocument FindById(int id)
        {
            UserDocument userDocument = repository.FindById(id);
            if (userDocument == null)
                return null;

            if (userDocument.IdentityCard != null)
            {
                userDocument.IdentityCard = "https://ifacilita.s3.us-east-2.amazonaws.com/" + userDocument.IdentityCard;
            }
            if (userDocument.SocialSecurityNumber != null)
            {
                userDocument.SocialSecurityNumber = "https://ifacilita.s3.us-east-2.amazonaws.com/" + userDocument.SocialSecurityNumber;
            }
            if (userDocument.SpouseIdentityCard != null)
            {
                userDocument.SpouseIdentityCard = "https://ifacilita.s3.us-east-2.amazonaws.com/" + userDocument.SpouseIdentityCard;
            }
            if (userDocument.SpouseSocialSecurityNumber != null)
            {
                userDocument.SpouseSocialSecurityNumber = "https://ifacilita.s3.us-east-2.amazonaws.com/" + userDocument.SpouseSocialSecurityNumber;
            }
            if (userDocument.MarriageCertificate != null)
            {
                userDocument.MarriageCertificate = "https://ifacilita.s3.us-east-2.amazonaws.com/" + userDocument.MarriageCertificate;
            }
            return userDocument;
        }

        public UserDocument FindByIdUser(int IdUser)
        {
            return FindById(IdUser);
        }

        public UserDocument Insert(UserDocument entity)
        {
            return InsertOrUpdate(entity);
        }

        public UserDocument Update(UserDocument entity)
        {
            return InsertOrUpdate(entity);
        }

        private UserDocument InsertOrUpdate(Entity.UserDocument userDocumentIn)
        {
            UserDocument userDocument = this.repository.FindByIdUser(userDocumentIn.IdUser.Value);
            User user = userService.FindById(userDocumentIn.IdUser.Value);
            if (userDocument == null)
            {
                Transaction transaction = this.transactionService.FindById(user.iddefailtTransaction.Value);

                if (user.IdUserProfile.Value == 1)
                    this.pushNotificationService.SendMessage(transaction.Seller.Value, "iFacilita - Documento", $"Comprador iniciou a importação dos documentos.", "", "https://ifacilita/logged-promise/upload-documents");
                else
                    this.pushNotificationService.SendMessage(transaction.IdUser.Value, "iFacilita - Documento", $"Vendedor iniciou a importação dos documentos.", "", "https://ifacilita/logged-promise/upload-documents");

                userDocument = this.repository.Insert(userDocumentIn);
            }


            if (userDocumentIn.IdentityCard != null)
            {

                this.historicService.Insert(new Historic()
                {
                    IdUser = user.Id,
                    Description = "Enviou copia digital do RG para a plataforma.",
                    Topic = "Sinal e Promessa",
                    IdTransaction = user.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

                userDocument.IdentityCard = s3.SaveFile(userDocumentIn.IdentityCard, new FileInfo(userDocumentIn.IdentityCard_FileName).Extension);
            }

            if (userDocumentIn.SocialSecurityNumber != null)
            {
                this.historicService.Insert(new Historic()
                {
                    IdUser = user.Id,
                    Description = "Enviou copia digital do CPF para a plataforma.",
                    Topic = "Sinal e Promessa",
                    IdTransaction = user.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

                userDocument.SocialSecurityNumber = s3.SaveFile(userDocumentIn.SocialSecurityNumber, new FileInfo(userDocumentIn.SocialSecurityNumber_FileName).Extension);
            }

            if (userDocumentIn.SpouseIdentityCard != null)
            {
                this.historicService.Insert(new Historic()
                {
                    IdUser = user.Id,
                    Description = "Enviou copia digital do RG do cônjuge para a plataforma.",
                    Topic = "Sinal e Promessa",
                    IdTransaction = user.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

                userDocument.SpouseIdentityCard = s3.SaveFile(userDocumentIn.SpouseIdentityCard, new FileInfo(userDocumentIn.SpouseIdentityCard_FileName).Extension);
            }
            if (userDocumentIn.SpouseSocialSecurityNumber != null)
            {
                this.historicService.Insert(new Historic()
                {
                    IdUser = user.Id,
                    Description = "Enviou copia digital do CPF do cônjuge para a plataforma.",
                    Topic = "Sinal e Promessa",
                    IdTransaction = user.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

                userDocument.SpouseSocialSecurityNumber = s3.SaveFile(userDocumentIn.SpouseSocialSecurityNumber, new FileInfo(userDocumentIn.SpouseSocialSecurityNumber_FileName).Extension);
            }

            if (userDocumentIn.MarriageCertificate != null)
            {
                this.historicService.Insert(new Historic()
                {
                    IdUser = user.Id,
                    Description = "Enviou copia digital da Certidão de Casamento para a plataforma.",
                    Topic = "Sinal e Promessa",
                    IdTransaction = user.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

                userDocument.MarriageCertificate = s3.SaveFile(userDocumentIn.MarriageCertificate, new FileInfo(userDocumentIn.MarriageCertificate_FileName).Extension);
            }
            userDocument = this.repository.Update(userDocument);

            return userDocument;

        }
    }
}
