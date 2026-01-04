using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.EmailService;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class TransactionCertificationService : ITransactionCertificationService
    {
        Repository.ITransactionCertificationRepository repository;
        Repository.ITransactionFlowRepository transactionFlowRepository;
        Service.IPushNotificationService pushNotificationService;
        Service.ITransactionService transactionService;
        Service.IUserService userService;
        Service.IPatrimonyService patrimonyService;
        Service.IAddressService addressService;
        Service.IUserSpouseService userSpouseService;

        IS3 s3;
        Common.IHttpClientFW httpClientFW;
        private readonly IConfiguration _configuration;

        IEnumerable<TransactionFlow> transactionFlows = null;

        public TransactionCertificationService(Repository.ITransactionCertificationRepository repository,
            Repository.ITransactionFlowRepository transactionFlowRepository,
            Service.IPushNotificationService pushNotificationService,
            Service.ITransactionService transactionService,
            Service.IUserService userService,
            Service.IPatrimonyService patrimonyService,
            Service.IAddressService addressService,
            Service.IUserSpouseService userSpouseService,
            IConfiguration configuration)
        {
            this.repository = repository;
            this.transactionFlowRepository = transactionFlowRepository;
            this.pushNotificationService = pushNotificationService;
            this.transactionService = transactionService;
            this.userService = userService;
            this.patrimonyService = patrimonyService;
            this.addressService = addressService;
            this.userSpouseService = userSpouseService;
            this.s3 = new Common.Impl.S3();

            _configuration = configuration;

        }
        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<TransactionCertification> FindAll()
        {
            throw new NotImplementedException();
        }

        public TransactionCertification FindById(int id)
        {
            return this.repository.FindById(id);
        }

        public IEnumerable<TransactionCertification> FindByIdtransaction(int idtransaction)
        {
            IEnumerable<TransactionCertification> certifications = this.repository.FindByIdtransaction(idtransaction);
            Entity.Transaction transaction = this.transactionService.FindById(idtransaction);
            transaction.Patrimony.Address = this.addressService.FindById(transaction.Patrimony.IdAddress.Value);

            if (transaction.Patrimony.Address.CitySig == "RJ")
            {
                List<Entity.TransactionCertification> transactionCertifications = certifications.Where(c => c.EcartorioId.Equals("Ifacilita.RPA") && c.CertificatePath == null).ToList();

                foreach (var item in transactionCertifications)
                {
                    RequestIFacilitaRPA(item);
                }
            }
            else if (transaction.Patrimony.Address.CitySig == "SP")
            {
                if (certifications.Count() == 0)
                    return null;

                this.httpClientFW = new Common.Impl.HttpClientFW($"{_configuration["eCartorio:SP:UrlApi"]}/Order");
                var result = httpClientFW.Get<JObject>(new string[] { certifications.First().EcartorioId });

                if (result.IsSuccessfully)
                {
                    if (result.Value["searchProtest"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Pesquisa de Protestos")).First();
                        if (result.Value["searchProtest"]["urlCertification"] != null && result.Value["searchProtest"]["urlCertification"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["searchProtest"]["urlCertification"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["searchProtest"]["urlBillet"] != null && result.Value["searchProtest"]["urlBillet"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["searchProtest"]["urlBillet"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["searchProtest"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["searchProtest"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }

                    if (result.Value["taxDebts"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Defeitos Ajuizados")).First();
                        if ((result.Value["defectsDefined"]["status"].ToString().Contains("Success") || result.Value["defectsDefined"]["status"].ToString().Contains("0")) && result.Value["defectsDefined"] != null && result.Value["defectsDefined"]["dataOrder"].HasValues)
                        {
                            if (result.Value["defectsDefined"]["dataOrder"]["urlCertificate"] != null && result.Value["defectsDefined"]["dataOrder"]["urlCertificate"].ToString() != "")
                            {
                                certification.CertificatePath = result.Value["defectsDefined"]["dataOrder"]["urlCertificate"].ToString();
                                certification.ExpirationDate = DateTime.Today.AddMonths(3);
                                this.repository.Update(certification);
                            }
                        }
                        else if (result.Value["defectsDefined"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["defectsDefined"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }

                    if (result.Value["taxDebts"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União")).First();
                        if (result.Value["taxDebts"]["status"].ToString().Contains("Success") && result.Value["taxDebts"]["idDocS3"] != null && result.Value["taxDebts"]["idDocS3"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["taxDebts"]["idDocS3"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["taxDebts"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["taxDebts"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }

                    if (result.Value["iptuDebts"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Débitos do IPTU")).First();
                        if (result.Value["iptuDebts"]["status"].ToString().Contains("Success") && result.Value["iptuDebts"]["urlCertification"] != null && result.Value["iptuDebts"]["urlCertification"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["iptuDebts"]["urlCertification"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["iptuDebts"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["iptuDebts"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }

                    if (result.Value["realOnus"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Ônus Reais")).First();
                        if (result.Value["realOnus"]["status"].ToString().Contains("Success") &&
                            result.Value["realOnus"]["s3patch"] != null &&
                            result.Value["realOnus"]["s3patch"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["realOnus"]["s3patch"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["realOnus"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["realOnus"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }


                    if (result.Value["propertyRegistrationData"].HasValues)
                    {
                        TransactionCertification certification = certifications.Where(c => c.CertificateName.Equals("Dados Cadastrais do Imóvel")).First();
                        if (result.Value["propertyRegistrationData"]["status"].ToString().Contains("Success") && result.Value["propertyRegistrationData"]["s3patch"] != null && result.Value["propertyRegistrationData"]["s3patch"].ToString() != "")
                        {
                            certification.CertificatePath = result.Value["propertyRegistrationData"]["s3patch"].ToString();
                            certification.ExpirationDate = DateTime.Today.AddMonths(3);
                            this.repository.Update(certification);
                        }
                        else if (result.Value["propertyRegistrationData"]["status"].ToString().Contains("Error"))
                        {
                            List<string> errors = new List<string>();
                            foreach (var error in result.Value["propertyRegistrationData"]["errors"])
                            {
                                errors.Add(error["message"].ToString());
                            }
                            certification.Errors = errors.ToArray();
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            var certificatesNull = certifications.Where(x => string.IsNullOrEmpty(x.CertificatePath)).ToList();

            if (transactionFlows == null)
                transactionFlows = transactionFlowRepository.findByIdTransaction(idtransaction);


            if (certificatesNull.Count() == 0)
            {
                //Aguardar recebimento das certidões - Vendedor
                UpdateTransactionFlow(idtransaction, 16, 2);

                //Aguardar recebimento das certidões - Comprador
                UpdateTransactionFlow(idtransaction, 18, 2);

                //ifacilita - Notificar todos os envolvidos com o prazo das certidões.
                if (transactionFlows.FirstOrDefault(x => x.IdplatformSubWorkflow.Equals(22)).Status != 2)
                {
                    //ifacilita - Notificar todos os envolvidos com o prazo das certidões.
                    UpdateTransactionFlow(idtransaction, 22, 2);

                    List<Tuple<string, string>> tos = new List<Tuple<string, string>>();

                    tos.Add(new Tuple<string, string>(transaction.User_Seller.Name, transaction.User_Seller.EMail));
                    tos.Add(new Tuple<string, string>(transaction.User.Name, transaction.User.EMail));

                    var resultSendBillet = new ClientEmailService(_configuration)
                        .SendMail(tos, null, null, null, new List<object>() { new { key = "CertificateDue", value = certifications.ToList()[0].ExpirationDate.Value.ToString("dd/MM/yyyy") } }, "CertificateDue");

                }

                //Aguardando disponibilidade das certidões do e-cartório.
                UpdateTransactionFlow(idtransaction, 23, 2);

                //Pré analise eletrônica dos documentos.
                UpdateTransactionFlow(idtransaction, 24, 2);

                //Documentos disponíveis para conferencia.
                UpdateTransactionFlow(idtransaction, 25, 2);

            }

            return certifications;
        }

        public IEnumerable<TransactionCertification> GetCertificationListForUploadDocument(int idtransaction)
        {
            IEnumerable<TransactionCertification> certificationsPending = this.GetListCertification(idtransaction);
            IEnumerable<TransactionCertification> certificationsExists = this.repository.FindByIdtransaction(idtransaction);
            List<TransactionCertification> certificationsToRequest = new List<TransactionCertification>();

            foreach (var item in certificationsPending)
            {
                if (certificationsExists.Count(c => c.CertificateName.Equals(item.CertificateName)) == 0)
                    certificationsToRequest.Add(item);
            }
            return certificationsToRequest;

        }

        public IEnumerable<TransactionCertification> GetListCertification(int idTransaction)
        {
            List<TransactionCertification> transactionCertifications = null;
            Entity.Transaction transaction = transactionService.FindById(idTransaction);
            transaction.Patrimony = patrimonyService.FindById(transaction.IdPatrimony);
            //transaction.Patrimony.Address = addressService.FindById(transaction.Patrimony.IdAddress.Value);
            if (transaction.Patrimony.Address.CitySig == "RJ")
            {
                this.httpClientFW = new Common.Impl.HttpClientFW("http://40.124.76.25:3990/api/kit/certiticatesbykit?idKit=2&municipio=Rio%20de%20Janeiro");
                var temp = this.httpClientFW.Get<object>(new[] { "" });
                var kit = ((Newtonsoft.Json.Linq.JArray)temp.Value);
                foreach (var act in kit)
                {
                    if (transactionCertifications == null)
                        transactionCertifications = new List<TransactionCertification>();

                    TransactionCertification certification = new TransactionCertification();
                    foreach (Newtonsoft.Json.Linq.JProperty field in act)
                    {

                        if (field.Name == "actDescription")
                            certification.CertificateName = field.Value.ToString();
                        else if (field.Name == "values")
                        {
                            foreach (float value in field.Value)
                            {
                                certification.Value += (decimal)value;
                            }
                            //certification.Value = field.Value.Sum();
                        }
                        else if (field.Name == "actId" && field.Value.ToString() == "44")
                        {
                            switch (transaction.Patrimony.NotaryNumber)
                            {
                                case 1: certification.Value = 126.56M; break;
                                case 2: certification.Value = 126.56M; break;
                                case 3: certification.Value = 126.55M; break;
                                case 4: certification.Value = 126.56M; break;
                                case 5: certification.Value = 126.49M; break;
                                case 6: certification.Value = 126.56M; break;
                                case 7: certification.Value = 126.58M; break;
                                case 8: certification.Value = 126.56M; break;
                                case 9: certification.Value = 126.56M; break;
                                case 10: certification.Value = 126.55M; break;
                                case 11: certification.Value = 126.56M; break;
                                case 12: certification.Value = 126.56M; break;

                                default:
                                    break;
                            }
                        }

                    }
                    transactionCertifications.Add(certification);
                }

                transactionCertifications.Add(new TransactionCertification()
                {
                    CertificateName = "Certidões Eletrônicas - Justiça Federal",
                    Value = 0M
                });
                transactionCertifications.Add(new TransactionCertification()
                {
                    CertificateName = "Certidão de situação fiscal e enfitêutica do IMÓVEL",
                    Value = 0M
                });
                transactionCertifications.Add(new TransactionCertification()
                {
                    CertificateName = "Certidão Negativa de Débitos Trabalhistas",
                    Value = 0M
                });
                transactionCertifications.Add(new TransactionCertification()
                {
                    CertificateName = "Certidão do bombeiro - FUNESBOM",
                    Value = 0M
                });



            }
            else if (transaction.Patrimony.Address.CitySig == "SP")
            {
                this.httpClientFW = new Common.Impl.HttpClientFW($"{_configuration["eCartorio:SP:UrlApi"]}/Certificate/all?city=São%20Paulo");
                var response = this.httpClientFW.Get<object>(new[] { "" });

                dynamic certificates = JArray.Parse(response.Value.ToString());

                if (transactionCertifications == null)
                    transactionCertifications = new List<TransactionCertification>();

                TransactionCertification certification = new TransactionCertification();

                foreach (var cert in certificates)
                {
                    var certificateType = cert.certiticateType.Value;
                    switch (cert.certiticateType.Value)
                    {
                        case "PropertyRegistrationData":
                            break;
                        case "IptuDebts":
                            break;
                        case "DefectsDefined":
                            break;
                        case "TaxDebts":
                            break;
                        case "SearchProtest":
                            break;
                        case "RealOnus":
                            break;
                        default:
                            break;
                    }

                    if (cert.description == "Pesquisa de Protestos")
                        cert.value = 292.60;
                    else if (cert.description == "Ônus Reais")
                        cert.value = 17.39;
                    else
                        cert.value = 0;

                    transactionCertifications.Add(new TransactionCertification()
                    {
                        CertificateName = cert.description,
                        Value = cert.value
                    });
                }
            }
            else
            {
                throw new Exception("Em construção");
            }

            IEnumerable<TransactionFlow> transactionFlows = this.transactionFlowRepository.findByIdTransaction(idTransaction);
            TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(20)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }

            return transactionCertifications;
        }

        public object MakeECartorioRequest(int idTransaction)
        {
            {
                IEnumerable<TransactionCertification> transactionCertifications = this.repository.FindByIdtransaction(idTransaction);
                foreach (var item in transactionCertifications)
                {
                    this.repository.Delete(item.Id);
                }
            }

            Entity.Transaction transaction = transactionService.FindById(idTransaction);

            if (transaction == null)
                return null;

            transaction.Patrimony = patrimonyService.FindById(transaction.IdPatrimony);

            string streetType = "";
            switch (transaction.Patrimony.StreetType)
            {
                case 1: streetType = "ALAMEDA"; break;
                case 2: streetType = "AVENIDA"; break;
                case 3: streetType = "BECO"; break;
                case 4: streetType = "CAMINHO"; break;
                case 5: streetType = "ESTRADA"; break;
                case 6: streetType = "ILHA"; break;
                case 7: streetType = "LADEIRA"; break;
                case 8: streetType = "LARGO"; break;
                case 9: streetType = "PRAÇA"; break;
                case 10: streetType = "PRAIA"; break;
                case 11: streetType = "RODOVIA"; break;
                case 12: streetType = "RUA"; break;
                case 13: streetType = "SERVIDAO"; break;
                case 14: streetType = "TRAVESSA"; break;
                case 15: streetType = "Vereda"; break;
                case 16: streetType = "VIA"; break;
                case 17: streetType = "VILA"; break;

            }

            if (transaction.Patrimony.Address.CitySig == "RJ")
            {
                List<object> dataActSearch = new List<object>();
                dataActSearch.Add(new
                {
                    nameSearch = transaction.User_Seller.Name + " " + transaction.User_Seller.LastName,
                    fatherName = transaction.User_Seller.FatherName,
                    matherName = transaction.User_Seller.MotherName,
                    birthDate = transaction.User_Seller.DateOfBirth,
                    cpfCnpj = transaction.User_Seller.SocialSecurityNumber.Value.ToString().PadLeft(11, '0')
                });

                if (transaction.User_Seller.UserSpouseSocialSecurityNumber.HasValue)
                {
                    Entity.UserSpouse spouse = this.userSpouseService.FindById(transaction.User_Seller.UserSpouseSocialSecurityNumber.Value);
                    dataActSearch.Add(new
                    {
                        nameSearch = spouse.Name,
                        fatherName = spouse.FatherName,
                        matherName = spouse.MotherName,
                        birthDate = spouse.DateOfBirth.Value,
                        cpfCnpj = spouse.SocialSecurityNumber.Value.ToString().PadLeft(11, '0')
                    });
                }

                var request = new
                {
                    cpf = _configuration["eCartorio:RJ:Document"],
                    name = _configuration["eCartorio:RJ:Name"],
                    email = _configuration["eCartorio:RJ:Email"],
                    urlCallback = _configuration["eCartorio:RJ:UrlCallback"],
                    actRegistry = new[]
                    {
                        new {
                            actId = 44,
                            registry = new int[] { 736 }
                        }
                    },
                    propertyDetails = new
                    {
                        registration = transaction.Patrimony.MunicipalRegistration,
                        cep = transaction.Patrimony.Address.ZipCode.Value.ToString().PadLeft(8, '0'),
                        city = transaction.Patrimony.Address.CitySig == "RJ" ? "Rio de Janeiro" : "",
                        streetType = streetType,
                        street = transaction.Patrimony.Address.Street,
                        number = transaction.Patrimony.Address.Number.ToString(),
                        neighborhood = transaction.Patrimony.Address.District
                    },
                    dataActSearch
                };

                this.httpClientFW = new Common.Impl.HttpClientFW($"{_configuration["eCartorio:RJ:UrlApi"]}/order/scripturekit");
                var temp = this.httpClientFW.Post<object, object>(new[] { "" }, request);
                if (temp.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                {
                    var kit = ((Newtonsoft.Json.Linq.JObject)temp.Value);


                    List<TransactionCertification> transactionCertifications = new List<TransactionCertification>(); ;
                    string id = "";
                    string billet = "";

                    foreach (var responseItem in kit)
                    {
                        if (responseItem.Key == "id")
                        {
                            id = responseItem.Value.ToString();
                        }
                        else if (responseItem.Key == "billet")
                        {
                            billet = responseItem.Value.ToString();
                        }
                        else if (responseItem.Key == "acts")
                        {
                            foreach (Newtonsoft.Json.Linq.JObject act in responseItem.Value)
                            {
                                TransactionCertification certificate = new TransactionCertification();
                                foreach (var item in act)
                                {
                                    if (item.Key == "certificate")
                                    {
                                        certificate.CertificateName = item.Value.ToString();
                                    }
                                    else if (item.Key == "register")
                                    {
                                        certificate.Notary = item.Value.ToString();
                                    }
                                }
                                certificate.Idtransaction = idTransaction;
                                certificate.ReceiptForecast = DateTime.Now.AddDays(7);
                                transactionCertifications.Add(certificate);
                            }
                        }
                    }

                    foreach (var item in transactionCertifications)
                    {
                        item.EcartorioId = id;
                    }

                    transactionCertifications.Add(new TransactionCertification()
                    {
                        CertificateName = "Certidões Eletrônicas - Justiça Federal",
                        Idtransaction = idTransaction,
                        ReceiptForecast = DateTime.Now.AddDays(7),
                        EcartorioId = "Ifacilita.RPA",
                        Value = 0M
                    });
                    transactionCertifications.Add(new TransactionCertification()
                    {
                        CertificateName = "Certidão de situação fiscal e enfitêutica do IMÓVEL",
                        Idtransaction = idTransaction,
                        ReceiptForecast = DateTime.Now.AddDays(7),
                        EcartorioId = "Ifacilita.RPA",
                        Value = 0M
                    });
                    transactionCertifications.Add(new TransactionCertification()
                    {
                        CertificateName = "Certidão Negativa de Débitos Trabalhistas",
                        Idtransaction = idTransaction,
                        ReceiptForecast = DateTime.Now.AddDays(7),
                        EcartorioId = "Ifacilita.RPA",
                        Value = 0M
                    });
                    transactionCertifications.Add(new TransactionCertification()
                    {
                        CertificateName = "Certidão do bombeiro - FUNESBOM",
                        Idtransaction = idTransaction,
                        ReceiptForecast = DateTime.Now.AddDays(7),
                        EcartorioId = "Ifacilita.RPA",
                        Value = 0M
                    });

                    foreach (var item in transactionCertifications)
                    {
                        this.repository.Insert(item);
                    }

                    IEnumerable<TransactionFlow> transactionFlows = this.transactionFlowRepository.findByIdTransaction(idTransaction);
                    TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(21)).FirstOrDefault();
                    transactionFlow.Status = 2;
                    transactionFlow.StatusChanged = DateTime.Now;
                    transactionFlowRepository.Update(transactionFlow);

                    return new { billet = billet };
                }
            }
            else if (transaction.Patrimony.Address.CitySig == "SP")
            {
                var request = new
                {
                    cpf = _configuration["eCartorio:SP:Document"],
                    name = _configuration["eCartorio:SP:Name"],
                    email = _configuration["eCartorio:SP:Email"],
                    urlCallback = _configuration["eCartorio:SP:UrlCallback"],
                    registry = new[]
                    {
                        new
                        {
                            certiticateType = "RealOnus",
                            registry = "1",
                            additionalInfo = new[]
                            {
                                new
                                {
                                    key = "IdUser",
                                    value = transaction.User_Seller.Id.ToString()
                                },
                                new
                                {
                                    key = "Protocolo", //TODO - Verificar quais informações deve vir aqui
                                    value = ""
                                },
                                new
                                {
                                    key="DocumentVisualization", //TODO - Verificar qual a melhor forma de passar essa informação.
                                    value="true"
                                }
                            }
                        },
                        new
                        {
                            certiticateType = "TaxDebts",
                            registry = "2",
                            additionalInfo = new[]
                            {
                                new
                                {
                                    key = "IdUserIfacilita",
                                    value = transaction.User_Seller.Id.ToString()
                                }
                            }
                        },
                        new
                        {
                            certiticateType =  "PropertyRegistrationData",
                            registry =  "0",
                            additionalInfo =  new[]
                            {
                                 new {
                                    key =  "Login",
                                    value = "33156538000149" //TODO - Verificar com o Ali se será criando um cadastro ou se será uma conta padrão - acredito que para cada SQL será um cadastro novo
                                },
                                new {
                                    key =  "Password",
                                    value = "Admin357/" //TODO - Verificar com o Ali se será criando um cadastro ou se será uma conta padrão - acredito que para cada SQL será um cadastro novo
                                },
                                new {
                                    key = "DateDoc",
                                    value = DateTime.Now.Year.ToString() //TODO - Verificar com o Ali de onde virá esse dado ou se será o ano corrente
                               }
                            }
                        },
                        new
                        {
                            certiticateType =  "SearchProtest",
                            registry =  "0",
                            additionalInfo = new[]
                            {
                                new
                                {
                                    key = "Approved",
                                    value = "false"
                                }
                            }
                        }
                    },
                    propertyDetails = new
                    {
                        registry = transaction.Patrimony.Registration,
                        cep = transaction.Patrimony.Address.ZipCode.Value.ToString().PadLeft(8, '0'),
                        city = transaction.Patrimony.Address.City.Description,
                        addressType = streetType,
                        address = transaction.Patrimony.Address.Street,
                        number = transaction.Patrimony.Address.Number.ToString(),
                        neighborhood = transaction.Patrimony.Address.District,
                        sql = transaction.Patrimony.SqlIptu,
                        idTransaction
                    },
                    dataSearch = new[]
                    {
                        new
                        {
                            name = transaction.User_Seller.Name + " " + transaction.User_Seller.LastName,
                            birthday= transaction.User_Seller.DateOfBirth,
                            cpf= transaction.User_Seller.SocialSecurityNumber.Value.ToString().PadLeft(11, '0'),
                            rg=transaction.User_Seller.IdentityCard,
                            email=transaction.User_Seller.EMail,
                            gender="Male" //TODO - Verificar se precisa solicitar no front-end
                        }
                    }
                };

                this.httpClientFW = new Common.Impl.HttpClientFW($"{_configuration["eCartorio:SP:UrlApi"]}/Order/all");
                var response = this.httpClientFW.Post<object, object>(new[] { "" }, request);

                var id = ((Newtonsoft.Json.Linq.JObject)response.Value).GetValue("id").ToString();

                List<Entity.TransactionCertification> certifications = GetListCertification(idTransaction).ToList();

                foreach (var certification in certifications)
                {
                    certification.EcartorioId = id;
                    certification.Idtransaction = idTransaction;
                    certification.ReceiptForecast = DateTime.Now.AddDays(3);
                    this.repository.Insert(certification);
                }

                IEnumerable<TransactionFlow> transactionFlows = this.transactionFlowRepository.findByIdTransaction(idTransaction);
                TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(21)).FirstOrDefault();
                transactionFlow.Status = 2;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }

            return new { billet = "https://ifacilita.s3.us-east-2.amazonaws.com/c54c277d-33d7-4d88-97f7-978fb1908dbd.pdf" }; //TODO
        }

        public TransactionCertification Insert(TransactionCertification entity)
        {
            IEnumerable<TransactionFlow> transactionFlows = this.transactionFlowRepository.findByIdTransaction(entity.Idtransaction);
            TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(20)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(21)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(22)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(23)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(24)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);

            transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(25)).FirstOrDefault();
            transactionFlow.Status = 2;
            transactionFlow.StatusChanged = DateTime.Now;
            transactionFlowRepository.Update(transactionFlow);


            if (entity.CertificateName.Contains("Justiça Federal"))
            {
                //k) Certidões Eletrônicas - Justiça Federal.

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    transaction.User_Seller.SocialSecurityNumber = 33486173839;
                    transaction.User.SocialSecurityNumber = 33486173839;


                    HttpResponseMessage respToken = client.GetAsync($"http://40.124.76.25:5720/JusticaFederal/{transaction.User_Seller.SocialSecurityNumber}/{transaction.User.SocialSecurityNumber}").Result;
                    //HttpResponseMessage respToken = client.GetAsync($"http://localhost:5720/JusticaFederal/{transaction.User_Seller.SocialSecurityNumber}/{transaction.User.SocialSecurityNumber}").Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2.Replace("\"", ""), ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("fiscal e enfitêutica do IMÓVEL"))
            {
                //d) Certidão de situação fiscal e enfitêutica do IMÓVEL;

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.GetAsync("http://40.124.76.25:5700/Efiteutica/" + transaction.PatrimonyMunicipalRegistration).Result;
                    //HttpResponseMessage respToken = client.GetAsync("http://localhost:5700/Efiteutica/" + transaction.PatrimonyMunicipalRegistration).Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("Certidão Negativa de Débitos Trabalhistas."))
            {
                //j) Certidão Negativa de Débitos Trabalhistas.

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = new TimeSpan(0, 10, 0);
                    HttpResponseMessage respToken = client.GetAsync($"http://40.124.76.25:5730/JusticaTrabalho/{transaction.User.SocialSecurityNumber}").Result;
                    //HttpResponseMessage respToken = client.GetAsync($"http://localhost:5730/JusticaTrabalho/{transaction.User.SocialSecurityNumber}").Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".pdf");
                        entity.ExpirationDate = DateTime.Today.AddDays(180);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("FUNESBOM"))
            {
                //l) Certidão do bombeiro - FUNESBOM

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.GetAsync("http://40.124.76.25:5710/Funesbom/" + transaction.PatrimonyMunicipalRegistration).Result;
                    //HttpResponseMessage respToken = client.GetAsync("http://localhost:5710/Funesbom/" + transaction.PatrimonyMunicipalRegistration).Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }

            return this.repository.Insert(entity);
        }

        public TransactionCertification RequestIFacilitaRPA(TransactionCertification entity)
        {
            if (entity.CertificateName.Contains("Justiça Federal"))
            {
                //k) Certidões Eletrônicas - Justiça Federal.

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    transaction.User_Seller.SocialSecurityNumber = 33486173839;
                    transaction.User.SocialSecurityNumber = 33486173839;


                    HttpResponseMessage respToken = client.GetAsync($"http://40.124.76.25:5720/JusticaFederal/{transaction.User_Seller.SocialSecurityNumber}/{transaction.User.SocialSecurityNumber}").Result;
                    //HttpResponseMessage respToken = client.GetAsync($"http://localhost:5720/JusticaFederal/{transaction.User_Seller.SocialSecurityNumber}/{transaction.User.SocialSecurityNumber}").Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2.Replace("\"", ""), ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("fiscal e enfitêutica do IMÓVEL"))
            {
                //d) Certidão de situação fiscal e enfitêutica do IMÓVEL;

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.GetAsync("http://40.124.76.25:5700/Efiteutica/" + transaction.PatrimonyMunicipalRegistration).Result;
                    //HttpResponseMessage respToken = client.GetAsync("http://localhost:5700/Efiteutica/" + transaction.PatrimonyMunicipalRegistration).Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("Certidão Negativa de Débitos Trabalhistas"))
            {
                //j) Certidão Negativa de Débitos Trabalhistas.

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = new TimeSpan(0, 10, 0);
                    HttpResponseMessage respToken = client.GetAsync($"http://40.124.76.25:5730/JusticaTrabalho/{transaction.User_Seller.SocialSecurityNumber}").Result;
                    //HttpResponseMessage respToken = client.GetAsync($"http://localhost:5730/JusticaTrabalho/{transaction.User_Seller.SocialSecurityNumber}").Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".pdf");
                        entity.ExpirationDate = DateTime.Today.AddDays(180);
                        entity.Received = DateTime.Now;
                    }
                }
            }
            else if (entity.CertificateName.Contains("FUNESBOM"))
            {
                //l) Certidão do bombeiro - FUNESBOM

                using (var client = new HttpClient())
                {
                    Transaction transaction = transactionService.FindById(entity.Idtransaction);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.GetAsync("http://40.124.76.25:5710/Funesbom/" + transaction.PatrimonyMunicipalRegistration).Result;
                    //HttpResponseMessage respToken = client.GetAsync("http://localhost:5710/Funesbom/" + transaction.PatrimonyMunicipalRegistration).Result;
                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string conteudo2 = respToken.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        entity.CertificatePath = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(conteudo2, ".png");
                        entity.ExpirationDate = DateTime.Today.AddDays(90);
                        entity.Received = DateTime.Now;
                    }
                }
            }



            return this.repository.Update(entity);
        }

        public TransactionCertification Update(TransactionCertification entity)
        {
            TransactionCertification transactionCertification = FindById(entity.Id);

            if (entity.Received.HasValue && !transactionCertification.Received.HasValue)
            {
                Transaction transaction = transactionService.FindById(transactionCertification.Idtransaction);
                this.pushNotificationService.SendMessage(transaction.IdUser.Value, "iFacilita - Sua certidão chegou", $"Sua certidão {entity.CertificateName} está disponível para conferencia.", "", "https://ifacilita/logged-certificates/read-accept-certificates/3");
                this.pushNotificationService.SendMessage(transaction.IdUser.Value, "iFacilita - Certidão chegou", $"A certidão {entity.CertificateName} está disponível para conferencia.", "", "https://ifacilita/logged-certificates/read-accept-certificates/3");
            }

            var listCertificates = FindByIdtransaction(entity.Idtransaction);
            var listCertificatesBuyerAccepted = listCertificates.Where(x => x.BuyerAccept != null && x.BuyerAccept.Value).ToList();
            var listCertificatesSellerAccepted = listCertificates.Where(x => x.SellerAccept != null && x.SellerAccept.Value).ToList();

            if ((listCertificates.Count() == listCertificatesBuyerAccepted.Count()))
            {
                UpdateTransactionFlow(entity.Idtransaction, 19, 2);
            }

            if ((listCertificates.Count() == listCertificatesSellerAccepted.Count()))
            {
                UpdateTransactionFlow(entity.Idtransaction, 17, 2);
            }

            return this.repository.Update(entity);
        }

        public void UploadCertification(string certificationName, TransactionCertification entity)
        {
            entity.CertificateFilename = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(entity.CertificateName, new FileInfo(entity.CertificateFilename).Extension);
            entity.CertificateName = certificationName;
            entity.CertificatePath = entity.CertificateFilename;
            entity.ReceiptForecast = DateTime.Now;
            this.repository.Insert(entity);
        }

        private bool UpdateTransactionFlow(int idTransaction, int idFlow, int status)
        {
            if (transactionFlows == null)
                transactionFlows = transactionFlowRepository.findByIdTransaction(idTransaction);

            TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(idFlow)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = status;
                transactionFlow.StatusChanged = DateTime.Now;
                transactionFlowRepository.Update(transactionFlow);
            }

            return true;
        }
    }
}
