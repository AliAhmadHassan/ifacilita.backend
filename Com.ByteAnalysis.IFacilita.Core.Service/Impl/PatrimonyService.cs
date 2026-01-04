using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.Impl
{
    public class PatrimonyService : IPatrimonyService
    {
        IPatrimonyRepository repository;
        IAddressRepository addressRepository;
        ITransactionRepository transactionRepository;
        ITransactionPaymentFormRepository transactionPaymentFormRepository;
        Service.IHistoricService historicService;

        private readonly IConfiguration _configuration;
        private readonly IRegistryService _registryService;
        private readonly ITransactionFlowRepository _transactionFlowRepository;
        public PatrimonyService(
            IPatrimonyRepository repository,
            IRegistryService registryService,
            ITransactionFlowRepository transactionFlowRepository,
            IConfiguration configuration,
            IAddressRepository addressRepository,
            ITransactionRepository transactionRepository,
            ITransactionPaymentFormRepository transactionPaymentFormRepository,
            IHistoricService historicService)
        {
            this.repository = repository;
            this.addressRepository = addressRepository;
            this.transactionRepository = transactionRepository;
            this.transactionPaymentFormRepository = transactionPaymentFormRepository;
            this.historicService = historicService;

            _configuration = configuration;
            _registryService = registryService;
            _transactionFlowRepository = transactionFlowRepository;
        }

        public void Delete(int id)
        {
            this.repository.Delete(id);
        }

        public IEnumerable<Patrimony> FindAll()
        {
            return repository.FindAll();
        }

        public Patrimony FindById(int id)
        {
            Entity.Patrimony patrimony = repository.FindById(id);
            if (patrimony != null)
                patrimony.Address = this.addressRepository.FindById(patrimony.IdAddress.Value);

            return patrimony;
        }

        public Patrimony Insert(Patrimony entity)
        {
            entity.Address = this.addressRepository.Insert(entity.Address);
            entity.IdAddress = entity.Address.Id;


            return repository.Insert(entity);
        }

        public object FindItbiRobot(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.GetAsync("http://ifacilita.com:5200/api/guiderequest/" + id).Result;
                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    return conteudo2;
                }
            }
            return null;
        }

        public object FindItbiSpRobot(string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.GetAsync($"{_configuration["ItbiSp:UrlApi"]}/ItbiSp/" + id).Result;
                //HttpResponseMessage respToken = client.GetAsync($"http://40.124.97.45:5811/api/ItbiSp/" + id).Result;
                //HttpResponseMessage respToken = client.GetAsync($"http://localhost:5811/api/ItbiSp/" + id).Result;
                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    return conteudo2;
                }
            }
            return null;
        }

        public void ItbiRobot(Transaction transaction, bool approved = false)
        {
            transaction = transactionRepository.FindById(transaction.Id);

            Entity.Address buyerAddress = addressRepository.FindById(transaction.User.IdAddress.Value);
            List<Entity.TransactionPaymentForm> transactionPaymentForms = this.transactionPaymentFormRepository.FindByIdtransaction(transaction.Id).ToList();

            decimal transacionValue = transaction.Signal.Value;
            transactionPaymentForms.ForEach(x => transacionValue += x.Plain.Value * x.Value.Value);

            transacionValue = transaction.Signal.Value + (transactionPaymentForms.LastOrDefault().Plain.Value * transactionPaymentForms.LastOrDefault().Value.Value);

            ResponseServerExternal response = null;
            switch (transaction.Patrimony.Address.CitySig)
            {
                case "RJ":
                    response = ProcessRJ(transaction, transacionValue, buyerAddress, approved);
                    break;
                case "SP":
                    response = ProcessSP(transaction, transacionValue, buyerAddress, approved);
                    break;
                default:
                    break;
            }

            if (response == null || response.HttpStatusCode != 200)
            {
                Console.WriteLine("Houve uma falha ao chamar o RPA. A mensagem do servidor foi: " + response.Message);
            }
            else
            {
                if (approved)
                {
                    _ = UpdateTransactionFlow(transaction.Id, 1037, 2);
                    _ = UpdateTransactionFlow(transaction.Id, 1041, 2);
                    _ = UpdateTransactionFlow(transaction.Id, 1042, 2);
                }
                else
                {
                    _ = UpdateTransactionFlow(transaction.Id, 1036, 2);
                    _ = UpdateTransactionFlow(transaction.Id, 1040, 2);
                }
            }
        }

        private bool UpdateTransactionFlow(int idTransaction, int idFlow, int status)
        {
            IEnumerable<TransactionFlow> transactionFlows = _transactionFlowRepository.findByIdTransaction(idTransaction);
            TransactionFlow transactionFlow = transactionFlows.Where(c => c.IdplatformSubWorkflow.Equals(idFlow)).FirstOrDefault();
            if (transactionFlow != null)
            {
                transactionFlow.Status = status;
                transactionFlow.StatusChanged = DateTime.Now;
                _transactionFlowRepository.Update(transactionFlow);
            }

            return true;
        }

        private ResponseServerExternal ProcessSP(Transaction transaction, decimal value, Address address, bool approved = false)
        {
            try
            {
                List<object> buyers = new List<object>();
                buyers.Add(new
                {
                    document = transaction.User.SocialSecurityNumber.ToString().PadLeft(11, '0'),
                    name = transaction.User.Name + " " + transaction.User.LastName
                });

                List<object> sellers = new List<object>();
                sellers.Add(new
                {
                    document = transaction.User_Seller.SocialSecurityNumber.ToString().PadLeft(11, '0'),
                    name = transaction.User_Seller.Name
                });

                var registry = _registryService.FindById(transaction.Patrimony.NotaryNumber);
                var registryNumber = registry.Name.Split('º', StringSplitOptions.RemoveEmptyEntries)[0];
                Console.WriteLine("O Cartório informado para o ITBI foi: " + registry.Name);
                Console.WriteLine("Tentado converter o valor: " + registryNumber);

                int outIntRegistry = 0;
                var registryStr = "";
                foreach (var c in registry.Name)
                {
                    Console.Out.WriteLine("Convertendo Caracter: " + c.ToString());
                    if(int.TryParse(c.ToString(),out outIntRegistry))
                    {
                        registryStr += c.ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                Console.Out.WriteLine("Cartório encontrado: " + registryStr);

                if (!int.TryParse(registryStr, out outIntRegistry))
                {
                    return new ResponseServerExternal()
                    {
                        HttpStatusCode = 400,
                        Message = "O número do cartório está errado."
                    };
                }

                string json = JsonConvert.SerializeObject(
                    new
                    {
                        transaction = 1,
                        iptu = transaction.Patrimony.MunicipalRegistration,
                        buyers = buyers,
                        sellers = sellers,
                        value,
                        financingType = transaction.Patrimony.FinancingType,
                        valueFinancing = transaction.Patrimony.ValueFinancing,
                        totality = transaction.Patrimony.TotalityTransfer,
                        proportion = transaction.Patrimony.ProportionTransfer,
                        publicScripture = transaction.Patrimony.PublicScripture,
                        dateEvent = transaction.Patrimony.DateEventScripture,
                        notesOffice = transaction.Patrimony.ScriptureNotesOffice,
                        uf = transaction.Patrimony.ScriptureUf,
                        city = transaction.Patrimony.ScriptureCity,
                        registry = outIntRegistry,
                        registration = transaction.Patrimony.Registration,
                        pending = true,
                        approved,
                        status = "pending",
                        urlCallback = $"http://ifacilita.com:5000/api/patrimony/{transaction.Id}/callback-itbi-sp",
                        transactionId = transaction.Id
                    }
                );

                using HttpClient client = new HttpClient();
                HttpResponseMessage respToken = client.PostAsync($"{_configuration["ItbiSp:UrlApi"]}/ItbiSp", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                //HttpResponseMessage respToken = client.PostAsync("http://40.124.97.45:5811/api/ItbiSp/", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                //HttpResponseMessage respToken = client.PostAsync("http://localhost:5811/api/ItbiSp/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

                if (respToken.IsSuccessStatusCode)
                {
                    var response = respToken.Content.ReadAsStringAsync().Result;
                    string iditbi_robot = response.Substring(7, 24);

                    Patrimony patrimony = transaction.Patrimony;
                    patrimony.IdItbiRobot = iditbi_robot;

                    repository.Update(patrimony);

                    return new ResponseServerExternal() { HttpStatusCode = 200, Message = "" };
                }
                else
                {
                    Console.Out.WriteLine(respToken.Content.ReadAsStringAsync().Result);
                    return new ResponseServerExternal() { HttpStatusCode = (int)respToken.StatusCode, Message = respToken.Content.ReadAsStringAsync().Result };
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                return new ResponseServerExternal() { HttpStatusCode = 500, Message = ex.Message };
            }
        }

        private ResponseServerExternal ProcessRJ(Transaction transaction, decimal value, Address address, bool approved = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    string json = JsonConvert.SerializeObject(new
                    {
                        id = "",
                        iptu = Int64.Parse(transaction.Patrimony.MunicipalRegistration),
                        value = value,
                        transactionNature = 1,
                        pal = "",
                        transferredPart = "100",
                        status = 1,
                        approved,
                        generation = new
                        {
                            purchaser = transaction.User.SocialSecurityNumber.ToString().PadLeft(11, '0'),
                            transmitted = transaction.User_Seller.SocialSecurityNumber.ToString().PadLeft(11, '0')
                        },
                        purchaserTransmitted = new
                        {
                            purchaserName = transaction.User.Name + " " + transaction.User.LastName,
                            purchaserOwnerSettings = transaction.Patrimony.IdPatrimonyAcquirerType.Value - 1,
                            address = address.Street,
                            number = address.Number,
                            neighborhood = address.District,
                            complement = address.Complement == null ? "" : address.Complement,
                            cep = address.ZipCode.ToString().PadLeft(8, '0'),
                            city = address.City.Description,
                            uf = address.City.Sig,
                            email = transaction.User_Seller.EMail,
                            ddd = transaction.User_Seller.DDD,
                            phoneNumber = transaction.User_Seller.MobileNumber.ToString(),
                            transmittedName = transaction.User_Seller.Name + " " + transaction.User_Seller.LastName,
                            transmittedOwnerSettings = transaction.Patrimony.IdPatrimonyTransmitterType.Value - 1,
                            countBedrooms = transaction.Patrimony.Bedrooms,
                            countBathroomExceptMaid = transaction.Patrimony.BathroomsExceptForMaids,
                            maidRoom = transaction.Patrimony.MaidsRoom,
                            bathroomMaid = transaction.Patrimony.MaidBathroom,
                            countParkingSpot = transaction.Patrimony.NumberOfCarSpaces,
                            balcony = transaction.Patrimony.Balcony,
                            propertyForeiro = transaction.Patrimony.ForeiroProperty,
                            floorPosition = transaction.Patrimony.FloorPosition.Value.ToString("00") + " andar",
                            elevator = transaction.Patrimony.Elevator.Value,
                            recreationArea = transaction.Patrimony.RecreationArea.Value
                        }
                    });

                    HttpResponseMessage respToken = client.PostAsync("http://ifacilita.com:5200/api/guiderequest", new StringContent(json, Encoding.UTF8, "application/json")).Result;

                    string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo2);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        string iditbi_robot = conteudo2.Substring(7, 24);

                        Patrimony patrimony = transaction.Patrimony;
                        patrimony.IdItbiRobot = iditbi_robot;

                        repository.Update(patrimony);

                        //5fa615788e142306b8009df5
                        //string idDoc = conteudo2.Substring(conteudo2.IndexOf(":") + 2, conteudo2.Length - conteudo2.IndexOf(":") - 4);
                        //transaction.IdDraft = idDoc;
                        //repository.Update(transaction);

                        //respToken = client.GetAsync("http://ifacilita.com:5100/api/transaction/" + idDoc).Result;
                        //conteudo2 = respToken.Content.ReadAsStringAsync().Result;

                        //TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(id).Where(c => c.IdplatformSubWorkflow.Equals(8)).FirstOrDefault();
                        //transactionFlow.StatusChanged = DateTime.Now;
                        //transactionFlow.Status = 2;


                        //this.transactionFlowRepository.Update(transactionFlow);

                        //return conteudo2.Substring(conteudo2.IndexOf("document\":\"") + 11, conteudo2.Length - conteudo2.IndexOf("document\":\"") - 12).Replace("\\n", "").Replace("\\r\\n", "");


                        this.historicService.Insert(new Historic()
                        {
                            IdUser = 5328,
                            Description = "Iniciamos o processo de ITBI",
                            Topic = "ITBI",
                            IdTransaction = transaction.Id,
                            Created = DateTime.Now
                        });

                        return new ResponseServerExternal() { HttpStatusCode = 200, Message = "" };
                    }
                    else
                    {
                        Console.Out.WriteLine(respToken.Content.ReadAsStringAsync().Result);
                        return new ResponseServerExternal() { HttpStatusCode = (int)respToken.StatusCode, Message = respToken.Content.ReadAsStringAsync().Result };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                return new ResponseServerExternal() { HttpStatusCode = 500, Message = ex.Message };
            }
        }

        public void ItbiRobotApproved(Transaction transaction)
        {
            ItbiRobot(transaction: transaction, approved: true);
        }

        #region Removido
        //public void ItbiRobotApproved(Transaction transaction)
        //{
        //    transaction = transactionRepository.FindById(transaction.Id);

        //    Entity.Address buyerAddress = addressRepository.FindById(transaction.User.IdAddress.Value);
        //    List<Entity.TransactionPaymentForm> transactionPaymentForms = this.transactionPaymentFormRepository.FindByIdtransaction(transaction.Id).ToList();

        //    decimal transacionValue = transaction.Signal.Value;

        //    foreach (var transactionPaymentForm in transactionPaymentForms)
        //    {
        //        transacionValue += transactionPaymentForm.Plain.Value * transactionPaymentForm.Value.Value;
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(
        //            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        string json = JsonConvert.SerializeObject(new
        //        {
        //            id = "",
        //            iptu = Int64.Parse(transaction.Patrimony.MunicipalRegistration),
        //            value = transacionValue,
        //            transactionNature = 1,
        //            pal = "",
        //            transferredPart = "100",
        //            status = 1,
        //            approved = true,
        //            generation = new
        //            {
        //                purchaser = transaction.User.SocialSecurityNumber.ToString().PadLeft(11, '0'),
        //                transmitted = transaction.User_Seller.SocialSecurityNumber.ToString().PadLeft(11, '0')
        //            },
        //            purchaserTransmitted = new
        //            {
        //                purchaserName = transaction.User.Name + " " + transaction.User.LastName,
        //                purchaserOwnerSettings = transaction.Patrimony.IdPatrimonyAcquirerType.Value - 1,
        //                address = buyerAddress.Street,
        //                number = buyerAddress.Number,
        //                neighborhood = buyerAddress.District,
        //                complement = buyerAddress.Complement == null ? "" : buyerAddress.Complement,
        //                cep = buyerAddress.ZipCode.ToString().PadLeft(8, '0'),
        //                city = buyerAddress.City.Description,
        //                uf = buyerAddress.City.Sig,
        //                email = transaction.User_Seller.EMail,
        //                ddd = transaction.User_Seller.DDD,
        //                phoneNumber = transaction.User_Seller.MobileNumber.ToString(),
        //                transmittedName = transaction.User_Seller.Name + " " + transaction.User_Seller.LastName,
        //                transmittedOwnerSettings = transaction.Patrimony.IdPatrimonyTransmitterType.Value - 1,
        //                countBedrooms = transaction.Patrimony.Bedrooms,
        //                countBathroomExceptMaid = transaction.Patrimony.BathroomsExceptForMaids,
        //                maidRoom = transaction.Patrimony.MaidsRoom,
        //                bathroomMaid = transaction.Patrimony.MaidBathroom,
        //                countParkingSpot = transaction.Patrimony.NumberOfCarSpaces,
        //                balcony = transaction.Patrimony.Balcony,
        //                propertyForeiro = transaction.Patrimony.ForeiroProperty,
        //                floorPosition = transaction.Patrimony.FloorPosition.Value.ToString("00") + " andar",
        //                elevator = transaction.Patrimony.Elevator.Value,
        //                recreationArea = transaction.Patrimony.RecreationArea.Value
        //            }
        //        });

        //        HttpResponseMessage respToken = client.PostAsync("http://ifacilita.com:5200/api/guiderequest", new StringContent(json, Encoding.UTF8, "application/json")).Result;

        //        string conteudo2 = respToken.Content.ReadAsStringAsync().Result;
        //        Console.WriteLine(conteudo2);

        //        if (respToken.StatusCode == HttpStatusCode.OK)
        //        {
        //            string iditbi_robot = conteudo2.Substring(7, 24);

        //            Patrimony patrimony = transaction.Patrimony;
        //            patrimony.IdItbiRobot = iditbi_robot;

        //            repository.Update(patrimony);

        //            //5fa615788e142306b8009df5
        //            //string idDoc = conteudo2.Substring(conteudo2.IndexOf(":") + 2, conteudo2.Length - conteudo2.IndexOf(":") - 4);
        //            //transaction.IdDraft = idDoc;
        //            //repository.Update(transaction);

        //            //respToken = client.GetAsync("http://ifacilita.com:5100/api/transaction/" + idDoc).Result;
        //            //conteudo2 = respToken.Content.ReadAsStringAsync().Result;

        //            //TransactionFlow transactionFlow = this.transactionFlowRepository.findByIdTransaction(id).Where(c => c.IdplatformSubWorkflow.Equals(8)).FirstOrDefault();
        //            //transactionFlow.StatusChanged = DateTime.Now;
        //            //transactionFlow.Status = 2;


        //            //this.transactionFlowRepository.Update(transactionFlow);

        //            //return conteudo2.Substring(conteudo2.IndexOf("document\":\"") + 11, conteudo2.Length - conteudo2.IndexOf("document\":\"") - 12).Replace("\\n", "").Replace("\\r\\n", "");


        //            this.historicService.Insert(new Historic()
        //            {
        //                IdUser = 5328,
        //                Description = "Iniciamos o processo de ITBI",
        //                Topic = "ITBI",
        //                IdTransaction = transaction.Id,
        //                Created = DateTime.Now
        //            });
        //        }
        //        else
        //        {

        //        }
        //    }
        //} 
        #endregion

        public Patrimony Update(Patrimony entity)
        {
            return repository.Update(entity);
        }

        internal class ResponseServerExternal
        {
            public int HttpStatusCode { get; set; }

            public string Message { get; set; }
        }
    }
}
