using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using Com.ByteAnalysis.IFacilita.Core.Service.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionService : ITransactionService
    {
        Repository.ITransactionRepository repository;
        Repository.IUserRepository userRepository;
        Repository.IPatrimonyRepository patrimonyRepository;
        Repository.IAddressRepository addressRepository;
        Repository.ITransactionPaymentFormRepository transactionPaymentFormRepository;
        Entity.ISmtpSettings smtp;
        Repository.ITransactionFlowRepository transactionFlowRepository;
        Repository.IRegistryRepository registryRepository;
        Repository.ITransactionCertificationRepository transactionCertificationRepository;
        Entity.IPathSettings pathSettings;
        Service.IPushNotificationService pushNotificationService;
        Common.IS3 s3 = new Common.Impl.S3();
        Service.IHistoricService historicService;

        private readonly IConfiguration _configuration;

        public TransactionService(ITransactionRepository repository, Repository.IUserRepository userRepository,
            Repository.IPatrimonyRepository patrimonyRepository, Repository.IAddressRepository addressRepository,
            Repository.ITransactionPaymentFormRepository transactionPaymentFormRepository,
            Repository.ITransactionFlowRepository transactionFlowRepository,
            Entity.ISmtpSettings smtp,
            Repository.IRegistryRepository registryRepository,
            Repository.ITransactionCertificationRepository transactionCertificationRepository,
            Entity.IPathSettings pathSettings,
            Service.IPushNotificationService pushNotificationService,
            Service.IHistoricService historicService, IConfiguration configuration)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.patrimonyRepository = patrimonyRepository;
            this.addressRepository = addressRepository;
            this.transactionPaymentFormRepository = transactionPaymentFormRepository;
            this.smtp = smtp;
            this.transactionFlowRepository = transactionFlowRepository;
            this.registryRepository = registryRepository;
            this.transactionCertificationRepository = transactionCertificationRepository;
            this.pathSettings = pathSettings;
            this.pushNotificationService = pushNotificationService;
            this.historicService = historicService;

            _configuration = configuration;
        }

        public void BuyerAgreeSignal(Transaction entity)
        {

            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(15)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;


            this.transactionFlowRepository.Update(transactionFlow);
        }

        public void BuyerRecivedSignalValue(Transaction entity)
        {

            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(1035)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;
            this.transactionFlowRepository.Update(transactionFlow);

            //transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(14)).FirstOrDefault();
            //transactionFlow.StatusChanged = DateTime.Now;
            //transactionFlow.Status = 2;
            //this.transactionFlowRepository.Update(transactionFlow);

            transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(15)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;
            this.transactionFlowRepository.Update(transactionFlow);

            transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(7)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;
            this.transactionFlowRepository.Update(transactionFlow);
        }

        public bool CallDocusignVFinal(int id)
        {
            Transaction transaction = this.repository.FindById(id);

            this.historicService.Insert(new Historic()
            {
                IdUser = transaction.Seller.Value,
                Description = "Solicitado assinatura da promessa via docusign",
                Topic = "Sinal e Promessa",
                IdTransaction = transaction.Id,
                Created = DateTime.Now
            });

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage respToken = client.GetAsync($"{_configuration["DraftApi:UrlBase"]}/transaction/{transaction.IdDraft}/pdf").Result;

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    string contractPdf = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");

                    //TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(id).Where(c => c.IdplatformSubWorkflow.Equals(14)).FirstOrDefault();
                    //transactionFlow.StatusChanged = DateTime.Now;
                    //transactionFlow.Status = 2;

                    Entity.UserSpouse userSellerSpouse = null;
                    Entity.UserSpouse userBuyerSpouse = null;
                    if (transaction.User_Seller.UserSpouseSocialSecurityNumber != null)
                        userSellerSpouse = this.userRepository.FindById(transaction.Seller.Value).UserSpouse;
                    if (transaction.User.UserSpouseSocialSecurityNumber != null)
                        userBuyerSpouse = this.userRepository.FindById(transaction.User.Id).UserSpouse;


                    //this.transactionFlowRepository.Update(transactionFlow);


                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    List<object> signers = new List<object>();
                    signers.Add(new
                    {
                        email = transaction.User.EMail,
                        name = transaction.User.Name + " " + transaction.User.LastName,
                        recipientId = 2,
                        routingOrder = 1
                    });

                    signers.Add(new
                    {
                        email = transaction.User_Seller.EMail,
                        name = transaction.User_Seller.Name + " " + transaction.User_Seller.LastName,
                        recipientId = 1,
                        routingOrder = 1
                    });

                    if (userSellerSpouse != null && userSellerSpouse.MaritalPropertySystems != 2)

                        signers.Add(new
                        {
                            email = userSellerSpouse.Email,
                            name = userSellerSpouse.Name,
                            recipientId = 3,
                            routingOrder = 1
                        });

                    if (userBuyerSpouse != null && userBuyerSpouse.MaritalPropertySystems != 2)
                        signers.Add(new
                        {
                            email = userBuyerSpouse.Email,
                            name = userBuyerSpouse.Name,
                            recipientId = 4,
                            routingOrder = 1
                        });

                    string json = JsonConvert.SerializeObject(new
                    {

                        id = "",
                        envelopeDocuSign = new
                        {
                            documents = new[]
                            {
                                new
                                {
                                    documentBase64 = contractPdf,
                                    documentId = transaction.Id,
                                    fileExtension = "pdf",
                                    name = "INSTRUMENTO PARTICULAR DE COMPRA E VENDA"
                                }
                            },
                            recipients = new[]
                            {
                                new
                                {
                                    signers
                                }
                            },
                            eventNotification = new
                            {
                                url = "https://ifacilita.com:5000/api/Transaction/callback-docusign"
                            },
                            emailSubject = "INSTRUMENTO PARTICULAR DE COMPRA E VENDA",
                            status = "sent"
                        }
                    });

                    respToken = client.PostAsync("https://ifacilitatech.com/api/envelopedocusign", new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {

                        this.pushNotificationService.SendMessage(transaction.Seller.Value, "iFacilita - Promessa/sinal", $"Processo de assinatura digital do Instrumento particular de compra e venda iniciado.", "", "https://ifacilita/logged-promise/draft-contract");
                        this.pushNotificationService.SendMessage(transaction.IdUser.Value, "iFacilita - Promessa/sinal", $"Processo de assinatura digital do Instrumento particular de compra e venda iniciado.", "", "https://ifacilita/logged-promise/draft-contract");
                    

                    StringBuilder stringBuilder = new StringBuilder();


                    this.historicService.Insert(new Historic()
                    {
                        IdUser = transaction.IdUser.Value,
                        Description = "Solicitado envio ao docusign com o documento para assinatura",
                        Topic = "Sinal e Promessa",
                        IdTransaction = transaction.Id,
                        Created = DateTime.Now
                    });

                    this.historicService.Insert(new Historic()
                    {
                        IdUser = transaction.Seller.Value,
                        Description = "Solicitado envio ao docusign com o documento para assinatura",
                        Topic = "Sinal e Promessa",
                        IdTransaction = transaction.Id,
                        Created = DateTime.Now
                    });

                    return true;
                    } else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CallDocusign(int id)
        {
            Transaction transaction = this.repository.FindById(id);

            using (var client = new HttpClient())
            {
                HttpResponseMessage respToken = client.GetAsync($"{_configuration["DraftApi:UrlBase"]}/transaction/" + transaction.IdDraft).Result;
                string conteudo2 = respToken.Content.ReadAsStringAsync().Result;

                string doc = conteudo2.Substring(conteudo2.IndexOf("document\":\"") + 11, conteudo2.Length - conteudo2.IndexOf("document\":\"") - 13).Replace("\\n", "").Replace("\\r\\n", "");

                //smtp.SendEmail(transaction.User.EMail, "INSTRUMENTO PARTICULAR DE COMPRA E VENDA", doc);
                //smtp.SendEmail(transaction.User_Seller.EMail, "INSTRUMENTO PARTICULAR DE COMPRA E VENDA", doc);

                TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(transaction.Id).Where(c => c.IdplatformSubWorkflow.Equals(10)).FirstOrDefault();
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlow.Status = 2;

                this.transactionFlowRepository.Update(transactionFlow);

                transactionFlow = this.transactionFlowRepository.findByIdTransaction(transaction.Id).Where(c => c.IdplatformSubWorkflow.Equals(12)).FirstOrDefault();
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlow.Status = 2;

                this.transactionFlowRepository.Update(transactionFlow);

                return CallDocusignVFinal(id);
            }

        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Transaction> FindAll()
        {
            return repository.FindAll();
        }

        public Transaction FindById(int id)
        {
            Transaction transaction = repository.FindById(id);
            if (transaction != null && transaction.PromiseVoucher != null)
            {
                transaction.PromiseVoucher = "https://ifacilita.s3.us-east-2.amazonaws.com/" + transaction.PromiseVoucher;
            }
            if (transaction != null && transaction.CertificateVoucher != null)
            {
                transaction.CertificateVoucher = "https://ifacilita.s3.us-east-2.amazonaws.com/" + transaction.CertificateVoucher;
            }
            if (transaction != null && transaction.ContractToken != null)
            {
                transaction.ContractToken = "https://ifacilita.s3.us-east-2.amazonaws.com/" + transaction.ContractToken;
            }

            return transaction;
        }

        public Transaction Insert(Transaction entity)
        {
            entity = repository.Insert(entity);

            if (entity.IdUser.HasValue)
            {
                User user = userRepository.FindById(entity.IdUser.Value);
                user.iddefailtTransaction = entity.Id;
                this.userRepository.Update(user);
            }

            if (entity.Seller.HasValue)
            {
                User user = userRepository.FindById(entity.Seller.Value);
                user.iddefailtTransaction = entity.Id;
                this.userRepository.Update(user);
            }

            return entity;
        }

        public string MakePromiseDocument(int id)
        {
            Entity.Transaction transaction = this.repository.FindById(id);

            if (transaction.IdDraft != null && transaction.IdDraft != "")
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage respToken = client.GetAsync($"{_configuration["DraftApi:UrlBase"]}/transaction/" + transaction.IdDraft).Result;
                    dynamic json = Newtonsoft.Json.Linq.JValue.Parse(respToken.Content.ReadAsStringAsync().Result);
                    var documentHtml = json.document?.ToString();
                    TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(id).Where(c => c.IdplatformSubWorkflow.Equals(8)).FirstOrDefault();
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlow.Status = 2;

                    this.transactionFlowRepository.Update(transactionFlow);

                    return documentHtml;
                }
            }


            Entity.Address patrimonyAddress = this.addressRepository.FindById(transaction.Patrimony.IdAddress.Value);
            Entity.Address sellerAddress = this.addressRepository.FindById(transaction.User_Seller.IdAddress.Value);
            Entity.Address buyerAddress = this.addressRepository.FindById(transaction.User.IdAddress.Value);
            Entity.UserSpouse userSellerSpouse = null;
            Entity.UserSpouse userBuyerSpouse = null;
            if (transaction.User_Seller.UserSpouseSocialSecurityNumber != null)
                userSellerSpouse = this.userRepository.FindById(transaction.Seller.Value).UserSpouse;
            if (transaction.User.UserSpouseSocialSecurityNumber != null)
                userBuyerSpouse = this.userRepository.FindById(transaction.User.Id).UserSpouse;
            IEnumerable<Entity.TransactionPaymentForm> transactionPaymentForms = this.transactionPaymentFormRepository.FindByIdtransaction(transaction.Id);

            decimal value = transaction.Signal.Value;
            StringBuilder paymentForm = new StringBuilder();

            paymentForm.Append("com sinal de " + transaction.Signal.Value.ToString("n2") + " e mais");

            foreach (var transactionPaymentForm in transactionPaymentForms)
            {
                value += (transactionPaymentForm.Plain.Value * transactionPaymentForm.Value.Value);
                paymentForm.Append($" {transactionPaymentForm.Plain} vezes de {transactionPaymentForm.Value.Value.ToString("n2")},");
            }



            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder sellerMaritalPropertySystems = new StringBuilder();
                if (transaction.User_Seller.IdUserSpouseType == 2)
                {
                    switch (userSellerSpouse.MaritalPropertySystems)
                    {
                        case 0: sellerMaritalPropertySystems.Append("sob regime de separação total de bens"); break;
                        case 1: sellerMaritalPropertySystems.Append("sob regime de comunhão parcial de bens"); break;
                        case 2: sellerMaritalPropertySystems.Append("sob regime de comunhão total de bens"); break;
                        default:
                            throw new Exception($"Type MaritalPropertySystems = {userBuyerSpouse.MaritalPropertySystems} isn't defined");
                            break;
                    }

                    sellerMaritalPropertySystems.Append($@", com {userSellerSpouse.Name}, portadora da cédula de identidade RG nº {userSellerSpouse.IdentityCard}, e inscrita no CPF/MF sob nº {userSellerSpouse.SocialSecurityNumber}");
                }


                StringBuilder buyerMaritalPropertySystems = new StringBuilder();
                if (transaction.User.IdUserSpouseType == 2)
                {
                    switch (userBuyerSpouse.MaritalPropertySystems)
                    {
                        case 0: sellerMaritalPropertySystems.Append("sob regime de separação total de bens"); break;
                        case 1: sellerMaritalPropertySystems.Append("sob regime de comunhão parcial de bens"); break;
                        case 2: sellerMaritalPropertySystems.Append("sob regime de comunhão total de bens"); break;
                        default:
                            throw new Exception($"Type MaritalPropertySystems = {userBuyerSpouse.MaritalPropertySystems} isn't defined");
                            break;
                    }
                    sellerMaritalPropertySystems.Append($@", com {userBuyerSpouse.Name}, portadora da cédula de identidade RG nº {userBuyerSpouse.IdentityCard}, e inscrita no CPF/MF sob nº {userBuyerSpouse.SocialSecurityNumber}");
                }
                string json = JsonConvert.SerializeObject(new
                {

                    patrimony = new
                    {
                        Registration = transaction.Patrimony.Registration,
                        RGINumber = "2",
                        Address = $"{patrimonyAddress.Street} {patrimonyAddress.Number}, {patrimonyAddress.Complement} - {patrimonyAddress.District} - {patrimonyAddress.City.Description}/{patrimonyAddress.City.Sig} - CEP {patrimonyAddress.ZipCode}"
                    },
                    seller = new
                    {
                        name = $"{transaction.User_Seller.Name} {transaction.User_Seller.LastName}",
                        socialSecurityNumber = transaction.User_Seller.SocialSecurityNumber.Value,
                        identityCard = transaction.User_Seller.IdentityCard,
                        nationality = transaction.User_Seller.Nationality,
                        spouseName = userSellerSpouse != null ? userSellerSpouse.Name : null,
                        address = $"{sellerAddress.Street} {sellerAddress.Number}, {sellerAddress.Complement} - {sellerAddress.District} - {sellerAddress.City.Description}/{sellerAddress.City.Sig} - CEP {sellerAddress.ZipCode}",
                        idUserSpouseType = transaction.User_Seller.IdUserSpouseType == null ? 1 : transaction.User_Seller.IdUserSpouseType,
                        maritalPropertySystems = sellerMaritalPropertySystems.ToString()
                    },
                    buyer = new
                    {
                        name = $"{transaction.User.Name} {transaction.User.LastName}",
                        socialSecurityNumber = transaction.User.SocialSecurityNumber,
                        identityCard = transaction.User.IdentityCard,
                        nationality = transaction.User.Nationality,
                        spouseName = userBuyerSpouse != null ? userBuyerSpouse.Name : null,
                        address = $"{buyerAddress.Street} {buyerAddress.Number}, {buyerAddress.Complement} - {buyerAddress.District} - {buyerAddress.City.Description}/{buyerAddress.City.Sig} - CEP {buyerAddress.ZipCode}",
                        idUserSpouseType = transaction.User.IdUserSpouseType == null ? 1 : transaction.User.IdUserSpouseType,
                        maritalPropertySystems = buyerMaritalPropertySystems.ToString()
                    },
                    value = value,
                    formOfPayment = paymentForm.ToString().Trim().Substring(0, paymentForm.ToString().Trim().Length - 1),
                    keyDeliveryConditions = transaction.KeyDelivery

                });

                HttpResponseMessage respToken = client.PostAsync($"{_configuration["DraftApi:UrlBase"]}/transaction", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                //HttpResponseMessage respToken = client.PostAsync("http://localhost:5100/api/transaction", new StringContent(json, Encoding.UTF8, "application/json")).Result;

                string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo2);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    //5fa615788e142306b8009df5
                    string idDoc = conteudo2.Substring(conteudo2.IndexOf(":") + 2, conteudo2.Length - conteudo2.IndexOf(":") - 4);
                    transaction.IdDraft = idDoc;
                    repository.Update(transaction);

                    respToken = client.GetAsync($"{_configuration["DraftApi:UrlBase"]}/transaction/" + idDoc).Result;
                    conteudo2 = respToken.Content.ReadAsStringAsync().Result;

                    TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(id).Where(c => c.IdplatformSubWorkflow.Equals(8)).FirstOrDefault();
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlow.Status = 2;

                    this.pushNotificationService.SendMessage(transaction.Seller.Value, "iFacilita - Promessa/sinal", $"Instrumento particular de compra e venda está disponivel.", "", "https://ifacilita/logged-promise/draft-contract");
                    this.pushNotificationService.SendMessage(transaction.IdUser.Value, "iFacilita - Promessa/sinal", $"Instrumento particular de compra e venda está disponivel.", "", "https://ifacilita/logged-promise/draft-contract");


                    this.transactionFlowRepository.Update(transactionFlow);

                    this.historicService.Insert(new Historic()
                    {
                        IdUser = transaction.IdUser.Value,
                        Description = "Disponibilizado o documento de sinal e promessa para leitura e assinatura",
                        Topic = "Sinal e Promessa",
                        IdTransaction = transaction.Id,
                        Created = DateTime.Now
                    });

                    return conteudo2.Substring(conteudo2.IndexOf("document\":\"") + 11, conteudo2.Length - conteudo2.IndexOf("document\":\"") - 12).Replace("\\n", "").Replace("\\r\\n", "");
                }
                else
                {

                }
            }
            return "";
        }

        public Transaction Update(Transaction entity)
        {
            Transaction transaction = this.repository.FindById(entity.Id);
            if (entity.PromiseVoucher != null && entity.PromiseVoucher.Contains("base64,"))
            {
                entity.PromiseVoucher = s3.SaveFile(entity.PromiseVoucher, new FileInfo(entity.PromiseVoucher_FileName).Extension);

                this.historicService.Insert(new Historic()
                {
                    IdUser = entity.IdUser.Value,
                    Description = "Envio o comprovante do pagamento de sinal",
                    Topic = "Sinal e Promessa",
                    IdTransaction = entity.Id,
                    Created = DateTime.Now
                });
            }
            else if (entity.PromiseVoucher_FileName != null)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(this.pathSettings.FilePath + $"user-document/{entity.IdUser}/{entity.PromiseVoucher_FileName}");
                entity.PromiseVoucher = info.FullName;
            }


            if (entity.CertificateVoucher != null && entity.CertificateVoucher.Contains("base64,"))
            {
                entity.CertificateVoucher = s3.SaveFile(entity.CertificateVoucher, new FileInfo(entity.CertificateVoucher_FileName).Extension);

                this.historicService.Insert(new Historic()
                {
                    IdUser = entity.Seller.Value,
                    Description = "Envio o comprovante da taxa do cartorio",
                    Topic = "Certidão",
                    IdTransaction = entity.Id,
                    Created = DateTime.Now
                });
            }
            else if (entity.CertificateVoucher_FileName != null)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(this.pathSettings.FilePath + $"user-document/{entity.IdUser}/{entity.CertificateVoucher_FileName}");
                entity.CertificateVoucher = info.FullName;
            }

            if (entity.ItbiVoucher != null && (entity.ItbiVoucher.Contains("base64,") || IsBase64(entity.ItbiVoucher)))
            {
                entity.ItbiVoucher = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(entity.ItbiVoucher, new FileInfo(entity.ItbiVoucher_FileName).Extension);

                this.historicService.Insert(new Historic()
                {
                    IdUser = entity.Seller.Value,
                    Description = "Envio o comprovante da taxa do ITBI",
                    Topic = "ITBI",
                    IdTransaction = entity.Id,
                    Created = DateTime.Now
                });
            }
            else if (entity.ItbiVoucher_FileName != null)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(Path.Combine(this.pathSettings.FilePath, $"user-document/{entity.IdUser}/{entity.ItbiVoucher_FileName}"));
                entity.ItbiVoucher = info.FullName;
            }

            if (transaction.PatrimonyMunicipalRegistration == null && entity.PatrimonyMunicipalRegistration != null)
            {
                if(entity.Seller.HasValue || entity.IdUser.HasValue)
                    this.historicService.Insert(new Historic()
                    {
                        IdUser = (transaction.Seller != null ? entity.Seller.Value : entity.IdUser.Value),
                        Description = "Cadastrou o Imóvel na plataforma",
                        Topic = "Cadastro",
                        IdTransaction = entity.Id,
                        Created = DateTime.Now
                    });
            }

            if ((transaction.Seller == null && entity.Seller != null) || (transaction.IdUser == null && entity.IdUser != null))
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(File.ReadAllText("SmtpModels/register.html"));
                stringBuilder = stringBuilder.Replace("{{link}}", $"https://ifacilita.com/register/{Guid.NewGuid()}-{transaction.Id}-{(transaction.Seller == null ? entity.Seller : entity.IdUser)}");
                stringBuilder = stringBuilder.Replace("{{name}}", $"{(entity.User == null ? entity.User_Seller.Name : entity.User.Name)}");

                this.historicService.Insert(new Historic()
                {
                    IdUser = (entity.Seller == null ? entity.IdUser.Value : entity.Seller.Value),
                    Description = "Enviado e-mail para cadastro do " + (entity.Seller == null ? "proprietario" : "comprador"),
                    Topic = "Cadastro",
                    IdTransaction = entity.Id,
                    Created = DateTime.Now
                });

                User userSendEMail = null;

                if (transaction.IdUser == null && transaction.Seller == null)
                {
                    if (entity.IdUser != null)
                    {
                        userSendEMail = entity.User;

                        TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(6)).FirstOrDefault();
                        transactionFlow.StatusChanged = DateTime.Now;
                        transactionFlow.Status = 2;
                        this.transactionFlowRepository.Update(transactionFlow);
                    }
                    else
                    {
                        userSendEMail = entity.User_Seller;

                        TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(7)).FirstOrDefault();
                        transactionFlow.StatusChanged = DateTime.Now;
                        transactionFlow.Status = 2;
                        this.transactionFlowRepository.Update(transactionFlow);
                    }
                }
                else
                {
                    if (transaction.IdUser == null)
                    {
                        userSendEMail = entity.User;

                        TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(6)).FirstOrDefault();
                        transactionFlow.StatusChanged = DateTime.Now;
                        transactionFlow.Status = 2;
                        this.transactionFlowRepository.Update(transactionFlow);
                    }
                    else
                    {
                        userSendEMail = entity.User_Seller;

                        TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(entity.Id).Where(c => c.IdplatformSubWorkflow.Equals(7)).FirstOrDefault();
                        transactionFlow.StatusChanged = DateTime.Now;
                        transactionFlow.Status = 2;
                        this.transactionFlowRepository.Update(transactionFlow);


                    }
                }


                smtp.SendEmail(userSendEMail.EMail, "Cadastro", stringBuilder.ToString());
            }

            if (entity.IdRegistry.HasValue)
            {
                IEnumerable<TransactionCertification> transactionCertifications = this.transactionCertificationRepository.FindByIdtransaction(entity.Id);
                Address patrimonyAddress = addressRepository.FindById(entity.Patrimony.IdAddress.Value);
                Registry registry = this.registryRepository.FindById(entity.IdRegistry.Value);



                this.historicService.Insert(new Historic()
                {
                    IdUser = entity.Seller.Value,
                    Description = "Escolheu o cartório " + registry.Name,
                    Topic = "Escritura",
                    IdTransaction = entity.Id,
                    Created = DateTime.Now
                });

                registry.Email1 = "phyrida@gmail.com";

                if (registry.Email1 != null)
                {
                    List<string> attanchments = new List<string>();
                    foreach (var certification in transactionCertifications)
                    {
                        attanchments.Add(certification.CertificatePath);
                    }

                    transaction.RegistryToken = Guid.NewGuid().ToString();
                    entity.RegistryToken = transaction.RegistryToken;

                    #region MyRegion
                    //StringBuilder stringBuilder = new StringBuilder();
                    //stringBuilder.Append($@"iFacilita,<br/>
                    //<br/>
                    //Srs., <br/>
                    //Segue pedido de escritura refente ao im&oacute;vel adquirido por {entity.User.Name}.<br/>
                    //<br/>
                    //<h2> Dados do Comprador</h2>
                    //Nome:{entity.User.Name} {entity.User.LastName}<br/>
                    //RG: {entity.User.IdentityCard}<br/>
                    //CPF: {entity.User.SocialSecurityNumber}
                    //<br/>
                    //<h2> Dados do Vendedor</h2>
                    //Nome:{entity.User_Seller.Name} {entity.User_Seller.LastName}<br/>
                    //RG: {entity.User_Seller.IdentityCard}<br/>
                    //CPF: {entity.User_Seller.SocialSecurityNumber}
                    //<br/>
                    //<h2> Dados do Im&oacute;vel</h2>
                    //Registro Municipal: {entity.Patrimony.MunicipalRegistration}<br/>
                    //Inscri&ccedil;&atilde;o: {entity.Patrimony.Registration}<br/>
                    //Bairro: {patrimonyAddress.District}<br/>
                    //Cep: {patrimonyAddress.ZipCode}<br/>
                    //Cidade: {patrimonyAddress.City.Description}/{patrimonyAddress.City.Sig}
                    //<h2>Certid&otilde;es</h2>
                    //");

                    //foreach (var certification in transactionCertifications)
                    //{
                    //    stringBuilder.Append($@"<a href='{certification.CertificatePath}' target='blank'>{certification.CertificateName}</a><br/>");
                    //}

                    //stringBuilder.Append($@"<br/>
                    //<br/>
                    //Segue link para enviar a escritura: <a href='https://ifacilita.com/recive-contract-document/{transaction.Id}/{transaction.RegistryToken}'>https://ifacilita.com/recive-contract-document/{transaction.Id}/{transaction.RegistryToken}</a><br/><br/>
                    //Atenciosamente,<br/>
                    //ifacilita.com"); 
                    #endregion

                    StringBuilder sbCertificate = new StringBuilder();
                    foreach (var cert in transactionCertifications)
                        sbCertificate.Append($"<span style='background-color: #fff; padding: 10px;margin-left: 10px;'><a href='{cert.CertificatePath}' target='blank'>{cert.CertificateName}</a></span>");

                    var stringHtml = $"<body style='padding-left: 50px; padding-right: 50px;margin: 0;'> <table style='border: none; width: 100%; height: 566px; color:#333333;'> <tbody style='background-color: #f7f7f7; padding: 10px;;'> <tr style='height: 18px;'> <td style='width: 100%; height: 94px; text-align: center; background-color: #1B2A3C; color: #ffffff;' colspan='2'> <h1>iFacilita.com</h1> <div style='margin-top: -20px;'>resolve toda e qualquer etapa envolvida na compra ou venda de um im&oacute;vel</span> </td></tr><tr style='height: 18px;'> <td style='height: 18px; width: 100%; padding: 10px;' colspan='2'><br><em>Srs.,</em><br/><em>Segue pedido de escritura refente ao im&oacute;vel adquirido por {entity.User.Name}</em></td></tr><tr style='height: 18px;'> <td style='width: 100%; height: 18px; padding:5px;' colspan='2'> <h2>Dados do Comprador:</h2> </td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Nome</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User.Name} {entity.User.LastName}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>RG</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User.IdentityCard}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>CPF</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User.SocialSecurityNumber}</td></tr><tr style='height: 62px;'> <td style='width: 100%; height: 62px; padding: 5px;' colspan='2'> <h2>Dados do Vendedor:</h2> </td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Nome</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User_Seller.Name}{entity.User_Seller.LastName}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>RG</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User_Seller.IdentityCard}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>CPF</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.User_Seller.SocialSecurityNumber}</td></tr><tr style='height: 18px;'> <td style='width: 100%; height: 18px; padding:5px;' colspan='2'> <h2>Dados do Im&oacute;vel:</h2> </td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Registro Municipal</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.Patrimony.MunicipalRegistration}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Inscri&ccedil;&atilde;o</td><td style='width: 98%; height: 18px; padding:5px;'>{entity.Patrimony.Registration}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Bairro</td><td style='width: 98%; height: 18px; padding:5px;'>{patrimonyAddress.District}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Cep</td><td style='width: 98%; height: 18px; padding:5px;'>{patrimonyAddress.ZipCode}</td></tr><tr style='height: 18px;'> <td style='width: 8%; height: 18px; padding:5px;'>Cidade</td><td style='width: 98%; height: 18px; padding:5px;'>{patrimonyAddress.City.Description}/{patrimonyAddress.City.Sig}</td></tr><tr style='height: 61px;'> <td style='width: 100%; height: 61px; padding: 5px;' colspan='2'> <h2>Certid&otilde;es:</h2> </td></tr><tr style='height: 50px;'> <td style='width: 100%; height: 50px; padding:5px; margin-right: 10px; margin-left: 10px;' colspan='2'>{sbCertificate.ToString()}</td></tr><tr style='height: 18px;'> <td style='height: 18px; width: 100%;text-align: center;' colspan='2'>Segue link para enviar a escritura: <br><a href='https://ifacilita.com/recive-contract-document/{transaction.Id}/{transaction.RegistryToken}' style='font-size:30px;'>Clique aqui para anexar a escritura solicitada</a><br/><br/> </td></tr><tr> <td style='width: 100%;text-align: center;' colspan='2'>Atenciosamente:</td></tr><tr> <td style='width: 100%; text-align: center;' colspan='2'>ifacilita.com</td></tr></tbody> </table></body>";

                    //smtp.SendEmail(registry.Email1, "Pedido de escritura", stringBuilder.ToString());
                    //smtp.SendEmail(registry.Email1, "Pedido de escritura", stringBuilder.ToString(), attanchments.ToArray());
                    smtp.SendEmail("phyrida@outlook.com", "Pedido de escritura", stringHtml.ToString());
                    smtp.SendEmail("haley_eng@hotmail.com", "Pedido de escritura", stringHtml.ToString());

                    var transactionFlows = transactionFlowRepository.findByIdTransaction(transaction.Id);

                    var transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(26)).FirstOrDefault();
                    transactionFlow.Status = 2;
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlowRepository.Update(transactionFlow);

                    transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(27)).FirstOrDefault();
                    transactionFlow.Status = 2;
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlowRepository.Update(transactionFlow);

                    transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(28)).FirstOrDefault();
                    transactionFlow.Status = 2;
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlowRepository.Update(transactionFlow);

                    transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(29)).FirstOrDefault();
                    transactionFlow.Status = 1;
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlowRepository.Update(transactionFlow);

                    this.historicService.Insert(new Historic()
                    {
                        IdUser = transaction.Seller.Value,
                        Description = "Enviado o e-mail para o cartório " + registry.Name + " com os documentos necessários",
                        Topic = "Escritura",
                        IdTransaction = entity.Id,
                        Created = DateTime.Now
                    });
                }
            }

            return repository.Update(entity);
        }

        private bool IsBase64(string base64)
        {
            try
            {
                var converted = Convert.FromBase64String(base64);
                return true;
            }
            catch { return false; }
        }

        public string ReciveContract(ContractViewModel contractViewModel)
        {
            Transaction transaction = repository.FindById(contractViewModel.IdTransaction);
            transaction.ContractToken = s3.SaveFile(contractViewModel.Base64File, new FileInfo(contractViewModel.FileName).Extension);
            repository.Update(transaction);

            var transactionFlows = transactionFlowRepository.findByIdTransaction(transaction.Id);

            var transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(29)).FirstOrDefault();
            if(transactionFlow != null) {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(30)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }
            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(31)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }
            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(32)).FirstOrDefault();

            if (transactionFlow != null)
            {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }
            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(33)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }

            this.historicService.Insert(new Historic()
            {
                IdUser = 5328,
                Description = "Disponibilizado escritura para leitura e aceite.",
                Topic = "Escritura",
                IdTransaction = contractViewModel.IdTransaction,
                Created = DateTime.Now
            });

            return "https://ifacilita.s3.us-east-2.amazonaws.com/" + transaction.ContractToken;
        }

        public Transaction InformKeyCondition(Transaction transaction)
        {
            TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(transaction.Id).Where(c => c.IdplatformSubWorkflow.Equals(1043)).FirstOrDefault();
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlow.Status = 2;


            this.transactionFlowRepository.Update(transactionFlow);

            return this.Update(transaction);
        }
    }
}
