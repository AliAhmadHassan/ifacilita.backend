using Com.ByteAnalysis.IFacilita.Draft.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace Com.ByteAnalysis.IFacilita.Draft.Service.Impl
{
    public class TransactionService : ITransactionService
    {
        Repository.ITransactionRepository repository;
        public TransactionService(Repository.ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public Transaction Create(Transaction transaction) {

            string document = File.ReadAllText("modelo sinal.html", Encoding.Default);
            document = document.Replace("{{Seller.Name}}", transaction.Seller.Name );
            document = document.Replace("{{Seller.Nationality}}", transaction.Seller.Nationality);
            document = document.Replace("{{Seller.Profession}}", transaction.Seller.Profession);
            switch (transaction.Seller.IdUserSpouseType)
            {
                case 1:
                    document = document.Replace("{{Seller.MaritalPropertySystems}}", "Solteiro(a)"); break;
                case 2:
                    document = document.Replace("{{Seller.MaritalPropertySystems}}", "Viúvo(a)"); break;
                case 3:
                    document = document.Replace("{{Seller.MaritalPropertySystems}}", "Casado(a), pelo regime da " + transaction.Seller.MaritalPropertySystems + ", com " + transaction.Seller.SpouseName); break;
                case 4:
                    document = document.Replace("{{Seller.MaritalPropertySystems}}", "Divorciado(a)"); break;
                default:
                    break;
            }
            document = document.Replace("{{Seller.IdentityCard}}", transaction.Seller.IdentityCard);
            document = document.Replace("{{Seller.ExpeditionLocal}}", transaction.Seller.ExpeditionLocal);
            document = document.Replace("{{Seller.ExpeditionDate}}", transaction.Seller.ExpeditionDate.ToString("dd/MM/yyyy"));
            document = document.Replace("{{Seller.SocialSecurityNumber}}", transaction.Seller.SocialSecurityNumber.ToString());
            document = document.Replace("{{Seller.Address}}", transaction.Seller.Address);


            document = document.Replace("{{Buyer.Name}}", transaction.Buyer.Name);
            document = document.Replace("{{Buyer.Nationality}}", transaction.Buyer.Nationality);
            document = document.Replace("{{Buyer.Profession}}", transaction.Buyer.Profession);
            switch (transaction.Buyer.IdUserSpouseType)
            {
                case 1:
                    document = document.Replace("{{Buyer.MaritalPropertySystems}}", "Solteiro(a)"); break;
                case 2:
                    document = document.Replace("{{Buyer.MaritalPropertySystems}}", "Viúvo(a)"); break;
                case 3:
                    document = document.Replace("{{Buyer.MaritalPropertySystems}}", "Casado(a), pelo regime da " + transaction.Buyer.MaritalPropertySystems + ", com " + transaction.Buyer.SpouseName); break;
                case 4:
                    document = document.Replace("{{Buyer.MaritalPropertySystems}}", "Divorciado(a)"); break;
                default:
                    break;
            }
            document = document.Replace("{{Buyer.IdentityCard}}", transaction.Buyer.IdentityCard);
            document = document.Replace("{{Buyer.ExpeditionLocal}}", transaction.Buyer.ExpeditionLocal);
            document = document.Replace("{{Buyer.ExpeditionDate}}", transaction.Buyer.ExpeditionDate.ToString("dd/MM/yyyy"));
            document = document.Replace("{{Buyer.SocialSecurityNumber}}", transaction.Buyer.SocialSecurityNumber.ToString());
            document = document.Replace("{{Buyer.Address}}", transaction.Buyer.Address);

            document = document.Replace("{{Transaction.Value}}", transaction.Value.ToString("n2"));
            document = document.Replace("{{Transaction.FormOfPayment}}", transaction.FormOfPayment);
            document = document.Replace("{{Transaction.Extension}}", Converter.toExtenso(transaction.Value));

            document = document.Replace("{{Patrimony.Registration}}", transaction.Patrimony.Registration);
            document = document.Replace("{{Patrimony.RGINumber}}", transaction.Patrimony.RGINumber);
            document = document.Replace("{{Patrimony.Address}}", transaction.Patrimony.Address);

            transaction.Document = Acerta(document);

            transaction = this.repository.Create(transaction);

            ConvertToPDF(transaction);

            return transaction;
        } 

        public List<Transaction> Get() => this.repository.Get();

        public Transaction Get(string id) => this.repository.Get(id);

        public void Remove(Transaction transactionIn) => this.repository.Remove(transactionIn);

        public void Remove(string id) => this.repository.Remove(id);

        public void Update(string id, Transaction transactionIn) => this.repository.Update(id, transactionIn);

        private void ConvertToPDF(Transaction transaction)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync("http://localhost:54229/home", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            html= Convert.ToBase64String(Convert.FromBase64String(transaction.Document))
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo2 =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo2);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    File.WriteAllBytes($"{transaction.Id}.pdf", Convert.FromBase64String(conteudo2.Substring(10, conteudo2.Length - 11)));
                }
                else
                {

                }
            }
        }

        public static string Acerta(string Dado)
        {
            string Mensagem = "";
            Mensagem = Dado;
            //Mensagem = Mensagem.Replace("&", "&amp;");
            Mensagem = Mensagem.Replace("ç", "&#231;");
            Mensagem = Mensagem.Replace("à", "&#224;");
            Mensagem = Mensagem.Replace("á", "&#225;");
            Mensagem = Mensagem.Replace("â", "&#226;");
            Mensagem = Mensagem.Replace("ã", "&#227;");
            Mensagem = Mensagem.Replace("å", "&#228;");
            Mensagem = Mensagem.Replace("ä", "&#229;");
            Mensagem = Mensagem.Replace("è", "&#232;");
            Mensagem = Mensagem.Replace("é", "&#233;");
            Mensagem = Mensagem.Replace("ê", "&#234;");
            Mensagem = Mensagem.Replace("ë", "&#235;");
            Mensagem = Mensagem.Replace("ì", "&#236;");
            Mensagem = Mensagem.Replace("í", "&#237;");
            Mensagem = Mensagem.Replace("î", "&#238;");
            Mensagem = Mensagem.Replace("ï", "&#239;");
            Mensagem = Mensagem.Replace("ð", "&#240;");
            Mensagem = Mensagem.Replace("ó", "&#243;");
            Mensagem = Mensagem.Replace("ô", "&#244;");
            Mensagem = Mensagem.Replace("õ", "&#245;");
            Mensagem = Mensagem.Replace("ö", "&#246;");
            Mensagem = Mensagem.Replace("ù", "&#249;");
            Mensagem = Mensagem.Replace("ú", "&#250;");
            Mensagem = Mensagem.Replace("û", "&#251;");
            Mensagem = Mensagem.Replace("ü", "&#252;");
            Mensagem = Mensagem.Replace("Ç", "&#199;");
            Mensagem = Mensagem.Replace("º", "&#186;");
            Mensagem = Mensagem.Replace("°", "&#176;");
            Mensagem = Mensagem.Replace("ª", "&#170;");
            Mensagem = Mensagem.Replace("Á", "&Aacute;");
            Mensagem = Mensagem.Replace("É", "&Eacute;");
            Mensagem = Mensagem.Replace("Í", "&Iacute;");
            Mensagem = Mensagem.Replace("Ó", "&Oacute;");
            Mensagem = Mensagem.Replace("Ú", "&Uacute;");
            Mensagem = Mensagem.Replace("Â", "&Acirc;");
            Mensagem = Mensagem.Replace("Ê", "&Ecirc;");
            Mensagem = Mensagem.Replace("Ô", "&Ocirc;");
            Mensagem = Mensagem.Replace("À", "&Agrave;");
            Mensagem = Mensagem.Replace("Ü", "&Uuml;");
            Mensagem = Mensagem.Replace("Ç", "&Ccedil;");
            Mensagem = Mensagem.Replace("Ã", "&Atilde;");
            Mensagem = Mensagem.Replace("Õ", "&Otilde;");
            Mensagem = Mensagem.Replace("Ñ", "&Ntilde;");
            

            return Mensagem;

        }
    }
}
