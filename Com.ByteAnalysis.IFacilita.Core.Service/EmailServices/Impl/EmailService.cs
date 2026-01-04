using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Impl
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };


                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                }

                return emails;
            }
        }

        public Tuple<bool, string> Send(EmailMessage emailMessage, string[] attachment)
        {
            var message = new MimeMessage();

            var formatOptions = FormatOptions.Default.Clone();
            formatOptions.AllowMixedHeaderCharsets = false;

            message.Headers.Replace(HeaderId.Subject, Encoding.UTF8, emailMessage.Subject);

            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address) { Encoding = Encoding.UTF8 }));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address) { Encoding = Encoding.UTF8 }));

            var builder = new BodyBuilder();
            builder.HtmlBody = emailMessage.Content;

            if (attachment != null)
                foreach (var at in attachment)
                {
                    if (!string.IsNullOrEmpty(at))
                        builder.Attachments.Add(at);
                }

            message.Body = builder.ToMessageBody();

            using (var emailClient = new SmtpClient())
            {
                try
                {
                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Tentando se conectar ao servidor SMTP" });
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, false);
                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Conexão com SMTP Realizada com sucesso" });

                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Autenticando no SMTP" });

                    //Remover se não estiver utilizando 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Autentica realizada com sucesso" });
                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Enviando Email" });
                    emailClient.Send(message);
                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "Email enviado com sucesso" });
                    emailClient.Disconnect(true);

                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllLines("EmailService" + "_" + DateTime.Now.ToString("ddMMyyyy") + ".log", new[] { "[ERROR] - " + ex.Message + ex.StackTrace });
                    return new Tuple<bool, string>(false, ex.Message);
                }
            }

            return new Tuple<bool, string>(true, "ok");
        }
    }
}
