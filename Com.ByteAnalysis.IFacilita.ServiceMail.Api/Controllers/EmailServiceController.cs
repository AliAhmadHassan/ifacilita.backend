using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto;
using Com.ByteAnalysis.IFacilita.ServiceMail.Api.Dto;
using Com.ByteAnalysis.IFacilita.ServiceMail.Service;
using Com.ByteAnalysis.IFacilita.ServiceMail.Service.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.ServiceMail.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServiceController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public EmailServiceController(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("sample")]
        public IActionResult SendMail([FromQuery] string email)
        {
            string body = string.Format(@"
                <h1>Olá Teste! {0}</h1>
                <br/>
                Seja bem vindo à ifacilita! <br/>
                <br/>
                Primeiramente gostaríamos de agradecer a preferência por nossa empresa. Muito obrigado! Faremos o possível para lhe oferecer um excelente atendimento.<br/>
                <br/>
                Seus dados cadastrais foram processados com sucesso e uma conta foi criada para você.<br/>
                <br/>
                Atenciosamente,<br/>
                ifacilita.com", DateTime.Now.ToLongDateString());

            _ = _emailService.Send(new EmailMessage()
            {
                Content = body,
                FromAddresses = new List<EmailAddress>() {
                        new EmailAddress(){ Name = "iFacilita Health", Address="ali.hassan@byteanalysis.com.br" }
                    },
                Subject = "Olá, bem Vindo ao iFacilita!",
                ToAddresses = new List<EmailAddress>() {
                        new EmailAddress(){ Address = email, Name = email.Split("@")[0] }
                    }
            }, null);

            return Ok(new { code = 200, message = "ok" });
        }

        [HttpPost]
        public IActionResult Post([FromBody] Email email)
        {
            try
            {
                List<EmailAddress> from = new List<EmailAddress>();
                List<EmailAddress> to = new List<EmailAddress>();

                if (email.From != null)
                {
                    foreach (var f in email.From)
                        from.Add(new EmailAddress() { Address = f.Email, Name = f.Name });
                }
                else
                {
                    from.Add(new EmailAddress() { Name = "iFacilita", Address = "ifacilita@aumsistemas.com.br" });
                }

                if (email.To != null)
                    foreach (var t in email.To)
                        to.Add(new EmailAddress() { Address = t.Email, Name = t.Name });

                if (to.Count == 0)
                    return BadRequest(new { code = 400, message = "Nenhum destinatário foi informado" });

                string message = email.Message;
                string subject = string.IsNullOrEmpty(email.Subject) ? "Assunto não informado" : email.Subject;

                switch (email.MessageType)
                {
                    case "Sample":

                        break;
                    case "Wellcome":
                        subject = _configuration[$"EmailType:{email.MessageType}:Subject"];

                        var contentMessage = _configuration[$"EmailType:{email.MessageType}:Message"];
                        if (!string.IsNullOrEmpty(contentMessage))
                            message = string.Format(contentMessage, string.Join(',', to.Select(x => x.Name).ToList()));

                        break;
                    case "BilletItbi":
                        subject = _configuration[$"EmailType:{email.MessageType}:Subject"];

                        var bodyBilletItbi = _configuration[$"EmailType:{email.MessageType}:Message"];
                        if (!string.IsNullOrEmpty(bodyBilletItbi))
                            message = string.Format(bodyBilletItbi, string.Join(',', to.Select(x => x.Name).ToList()), email.OtherFields.FirstOrDefault(x => x.Key == "UrlBillet").Value);
                        
                        break;
                    case "CertificateDue":
                        subject = _configuration[$"EmailType:{email.MessageType}:Subject"];

                        var bodyBilletcertificate = _configuration[$"EmailType:{email.MessageType}:Message"];
                        if (!string.IsNullOrEmpty(bodyBilletcertificate))
                            message = string.Format(bodyBilletcertificate, string.Join(',', to.Select(x => x.Name).ToList()), email.OtherFields.FirstOrDefault(x => x.Key == "CertificateDue").Value);

                        break;
                    default:
                        break;
                }

                _ = _emailService.Send(new EmailMessage()
                {
                    Content = message,
                    FromAddresses = from,
                    Subject = subject,
                    ToAddresses = to
                }, null);

                return Ok(new { code = 200, message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 500, ex.Message });
            }
        }
    }
}
