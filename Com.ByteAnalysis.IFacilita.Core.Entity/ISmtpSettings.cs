using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class SmtpSettings: ISmtpSettings
    {
        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool DefaultCredentials { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string SendEmail(string Destinatario, string Assunto, string enviaMensagem, string[] attachments = null)
        {
            try
            {
                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(Destinatario);

                // Se o email não é validao retorna uma mensagem
                if (bValidaEmail == false)
                    return "Email do destinatário inválido: " + Destinatario;

                //// cria uma mensagem
                //byte[] bytesAssunto = Encoding.Default.GetBytes(Assunto);
                //var _subject= Encoding.UTF8.GetString(bytesAssunto);

                //byte[] bytesEnviaMensagem = Encoding.Default.GetBytes(enviaMensagem);
                //var _body = Encoding.UTF8.GetString(bytesEnviaMensagem);

                MailMessage mensagemEmail = new MailMessage(this.From, Destinatario, Assunto, enviaMensagem);
               
                mensagemEmail.HeadersEncoding= Encoding.UTF8;
                mensagemEmail.BodyEncoding = Encoding.UTF8;
                mensagemEmail.BodyTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mensagemEmail.SubjectEncoding = Encoding.UTF8;

                mensagemEmail.IsBodyHtml = true;

                if (attachments != null)
                {
                    foreach (string attachment in attachments)
                    {
                        mensagemEmail.Attachments.Add(new Attachment(attachment));
                    }
                }
                
                SmtpClient client = new SmtpClient(this.Host, 587);
                
                client.EnableSsl = true;
                NetworkCredential cred = new NetworkCredential(this.UserName, this.Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio

                // inclui as credenciais
                client.UseDefaultCredentials = true;
                client.Credentials = cred;

                // envia a mensagem
                client.Send(mensagemEmail);

                return "Mensagem enviada para  " + Destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                string erro = ex.InnerException.ToString();
                return ex.Message.ToString() + erro;
            }
        }

        /// <summary>
        /// Confirma a validade de um email
        /// </summary>
        /// <param name="enderecoEmail">Email a ser validado</param>
        /// <returns>Retorna True se o email for valido</returns>
        private bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regulara para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_0-9]+?\.[a-zA-Z0-9]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(texto_Validar))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public interface ISmtpSettings
    {
        string From { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        bool DefaultCredentials { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        string SendEmail(string Destinatario, string Assunto, string enviaMensagem, string[] attachments = null);

    }
}
