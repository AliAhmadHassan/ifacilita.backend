using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto;
using Com.ByteAnalysis.IFacilita.Core.Service.Exceptions;
using Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IUserSpouseService userSpouseService;
        private readonly ISmtpSettings smtp;
        private readonly ITransactionService transactionService;
        private readonly IHistoricService historicService;
        private readonly IEmailService _emailService;
        private readonly IClientEmailService _clientEmailService;

        private readonly IClientApiPaymentsGateway _paymentsGateway;

        public UserService(IUserRepository repository,
            ITransactionRepository transactionRepository,
            IAddressRepository addressRepository,
            IUserSpouseService userSpouseService,
            ISmtpSettings smtp,
            ITransactionService transactionService,
            IEmailService emailService,
            IClientEmailService clientEmailService,
            IClientApiPaymentsGateway paymentsGateway,
            IHistoricService historicService)
        {
            this.repository = repository;
            this.transactionRepository = transactionRepository;
            this.addressRepository = addressRepository;
            this.userSpouseService = userSpouseService;
            this.smtp = smtp;
            this.transactionService = transactionService;
            this.historicService = historicService;

            _emailService = emailService;
            _clientEmailService = clientEmailService;
            _paymentsGateway = paymentsGateway;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<User> FindAll()
        {
            IEnumerable<User> users = repository.FindAll();

            foreach (var user in users)
            {
                user.Password = "";
            }

            return users;
        }

        public User FindById(int id)
        {
            User temp = repository.FindById(id);
            if (temp != null)
                temp.Password = "";

            return temp;
        }

        public User FindBySocialLoginAuthorizationCode(string authorizationCode)
        {
            Entity.User temp = this.repository.FindBySocialLoginAuthorizationCode(authorizationCode);

            if (temp != null)
                temp.Password = "";

            return temp;
        }

        public User Insert(User entity)
        {
            if (entity.Address != null)
            {
                if (entity.Address.Id != 0)
                    this.addressRepository.Update(entity.Address);
                else
                    entity.IdAddress = this.addressRepository.Insert(entity.Address).Id;
            }

            if (entity.UserSpouse != null && entity.UserSpouse.SocialSecurityNumber.HasValue)
            {
                if (userSpouseService.FindById(entity.UserSpouse.SocialSecurityNumber.Value) == null)
                {

                    if (entity.iddefailtTransaction.HasValue)
                        this.historicService.Insert(new Historic()
                        {
                            IdUser = entity.Id,
                            Description = "Cadastro do(a) cônjuge",
                            Topic = "Cadastro",
                            IdTransaction = entity.iddefailtTransaction.Value,
                            Created = DateTime.Now
                        });

                    this.userSpouseService.Insert(entity.UserSpouse);
                    entity.UserSpouseSocialSecurityNumber = entity.UserSpouse.SocialSecurityNumber.Value;
                }
                else
                {
                    if (entity.iddefailtTransaction.HasValue)
                        this.historicService.Insert(new Historic()
                        {
                            IdUser = entity.Id,
                            Description = "Alterou os dados do(a) cônjuge",
                            Topic = "Cadastro",
                            IdTransaction = entity.iddefailtTransaction.Value,
                            Created = DateTime.Now
                        });


                    this.userSpouseService.Update(entity.UserSpouse);
                    entity.UserSpouseSocialSecurityNumber = entity.UserSpouse.SocialSecurityNumber.Value;
                }
            }

            #region removed

            //string body = @$"
            //<h1>Olá {entity.Name}!</h1>
            //<br/>
            //Seja bem vindo à ifacilita! <br/>
            //<br/>
            //Primeiramente gostaríamos de agradecer a preferência por nossa empresa. Muito obrigado! Faremos o possível para lhe oferecer um excelente atendimento.<br/>
            //<br/>
            //Seus dados cadastrais foram processados com sucesso e uma conta foi criada para você.<br/>
            //<br/>
            //Atenciosamente,<br/>
            //ifacilita.com
            //"; 
            #endregion

            Entity.User userExist = this.repository.FindByEMail(entity.EMail);
            User temp = null;

            if (userExist == null)
            {
                Tuple<string, string> to = new Tuple<string, string>(entity.Name, entity.EMail);
                _clientEmailService.SendMail(new List<Tuple<string, string>>() { to }, null, null, null, null);
                temp = repository.Insert(entity);

            }
            else
            {
                entity.Id = userExist.Id;
                entity.SocialLoginAuthorizationCode = userExist.SocialLoginAuthorizationCode;
                temp = repository.Update(entity);
            }

            //Cadastra o usuário na plataforma de pagamentos para poder gerar novas faturas 
            if (entity.Address != null && entity.Address.ZipCode != null)
            {
                var clientPayment = new ClientDto()
                {
                    Active = true,
                    Cep = entity.Address.ZipCode?.ToString("00000000"),
                    Number = entity.Address.Number?.ToString(),
                    Country = "Brasil",
                    Name = $"{entity.Name} {entity.LastName}",
                    Email = entity.EMail,
                    Document = entity.SocialSecurityNumber?.ToString("00000000000"),
                    Phone = entity.MobileNumber?.ToString(),
                    Ddd = entity.DDD?.ToString()
                };

                var responsePayment = _paymentsGateway.CreateClientAsync(clientPayment).Result;
                entity.PaymentExtenalClientId = responsePayment.ExternalId;
                entity.PaymentClientId = responsePayment.Id;

                //Atualiza o usuário com os dados de pagamentos
                repository.Update(entity);
            }

            temp.Password = "";

            return temp;
        }

        public User InsertWithSocialLogin(User user)
        {
            Entity.User temp = this.repository.FindByEMail(user.EMail);

            try
            {
                if (temp == null)
                {
                    temp = Insert(user);
                }
                else
                {
                    temp.iddefailtTransaction = user.iddefailtTransaction;
                    temp.SocialLoginAuthorizationCode = user.SocialLoginAuthorizationCode;
                    temp = Update(temp);
                }

                if (temp != null)
                    temp.Password = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Houve uma falha ao tentar enviar o email de boas vindas para o usuário. " + ex.Message);
            }

            return temp;
        }

        public User Login(User user)
        {
            if (user.EMail != null && user.EMail != "")
            {
                User userIn = this.repository.FindByEMail(user.EMail);

                if (userIn == null)
                    throw new NotFoundException("Usuario não encontrado.");

                if (userIn.EMail == user.EMail && userIn.Password == user.Password)
                {
                    userIn.Password = "";
                    return userIn;
                }
                else
                {
                    throw new BadRequestException("Usuario ou senha invalida.");
                }
            }
            else
            {
                throw new BadRequestException("E-Mail não informado.");
            }
            return null;
        }

        public void MissPassword(Entity.User user)
        {
            User userTemp = null;

            if (user.EMail != "")
                userTemp = this.repository.FindByEMail(user.EMail);

            if (userTemp == null && user.SocialSecurityNumber.HasValue)
                userTemp = this.repository.FindBySocialSecurityNumber(user.SocialSecurityNumber.Value);

            if (userTemp == null)
                throw new Exception("Usuario não encontrado.");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($@"iFacilita,<br/>
<br/>
Você esta recebendo este e-mail para alterar a senha na plataforma Escritura Fácil, portal de automação em <br/>
compra e venda de imóveis.<br/>
<br/>
Favor <a href='https://ifacilita.com/register/{Guid.NewGuid()}-{userTemp.iddefailtTransaction}-{userTemp.Id}/{Guid.NewGuid()}' target='blank'>clicar aqui</a> para alterar sua senha.<br/>
<br/>
<br/>
Atenciosamente,<br/>
ifacilita.com");

            smtp.SendEmail(userTemp.EMail, "iFacilita - recuperação de senha.", stringBuilder.ToString());
        }

        public User Update(User entity)
        {
            if (entity.Address != null)
            {
                if (entity.Address.Id != 0)
                    this.addressRepository.Update(entity.Address);
                else
                    entity.IdAddress = this.addressRepository.Insert(entity.Address).Id;
            }

            if (entity.UserSpouse != null && entity.UserSpouse.SocialSecurityNumber.HasValue)
            {
                if (userSpouseService.FindById(entity.UserSpouse.SocialSecurityNumber.Value) == null)
                {
                    this.userSpouseService.Insert(entity.UserSpouse);
                    entity.UserSpouseSocialSecurityNumber = entity.UserSpouse.SocialSecurityNumber.Value;
                }
                else
                {
                    this.userSpouseService.Update(entity.UserSpouse);
                    entity.UserSpouseSocialSecurityNumber = entity.UserSpouse.SocialSecurityNumber.Value;
                }
            }

            User oldRegistry = repository.FindById(entity.Id);
            User temp = repository.Update(entity);
            temp.Password = "";

            if (oldRegistry.EMail != temp.EMail)
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(System.IO.File.ReadAllText("SmtpModels/register.html"));
                stringBuilder = stringBuilder.Replace("{{link}}", $"https://ifacilita.com/register/{Guid.NewGuid()}-{entity.iddefailtTransaction}-{entity.Id}");
                stringBuilder = stringBuilder.Replace("{{name}}", $"{entity.Name} {entity.LastName}");


                smtp.SendEmail(entity.EMail, "Cadastro", stringBuilder.ToString());

            }

            if (entity.iddefailtTransaction != null)
                this.historicService.Insert(new Historic()
                {
                    IdUser = entity.Id,
                    Description = "Alterou os dados cadastrais.",
                    Topic = "Cadastro",
                    IdTransaction = entity.iddefailtTransaction.Value,
                    Created = DateTime.Now
                });

            //Cadastra o usuário na plataforma de pagamentos para poder gerar novas faturas 
            if ((entity.Address != null && entity.Address.ZipCode != null) && (string.IsNullOrEmpty(entity.PaymentExtenalClientId) && entity.PaymentClientId == null))
            {
                var clientPayment = new ClientDto()
                {
                    Active = true,
                    Cep = entity.Address.ZipCode?.ToString("00000000"),
                    Number = entity.Address.Number?.ToString(),
                    Country = "Brasil",
                    Name = $"{entity.Name} {entity.LastName}",
                    Email = entity.EMail,
                    Document = entity.SocialSecurityNumber?.ToString("00000000000"),
                    Phone = entity.MobileNumber?.ToString(),
                    Ddd = entity.DDD?.ToString()
                };

                var responsePayment = _paymentsGateway.CreateClientAsync(clientPayment).Result;
                entity.PaymentExtenalClientId = responsePayment.ExternalId;
                entity.PaymentClientId = responsePayment.Id;

                //Atualiza o usuário com os dados de pagamentos
                repository.Update(entity);
            }

            return temp;
        }
    }
}
