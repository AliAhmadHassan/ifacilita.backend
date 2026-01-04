using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Core.Entity;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices;
using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IClientEmailService _clientEmailService;

        ISmtpSettings _smtp;
        private readonly IEmailService _emailService;

        public HealthController(IConfiguration configuration, ISmtpSettings smtp, IEmailService emailService, IClientEmailService clientEmailService)
        {
            _configuration = configuration;
            _smtp = smtp;
            _emailService = emailService;
            _clientEmailService = clientEmailService;
        }

        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            return Ok(new[] { 
                new { date = "16/04/2021", version = "1.0.0", description="Criação desse controle de versão" } ,
                new { date = "23/06/2021", version = "1.0.1", description="Alteração do usuário de requisição do eCartório RJ" } ,
            });
        }

        [HttpGet("ecartoriosp")]
        public async Task<IActionResult> GeteCartorioSp()
        {
            var urlGet = $"{_configuration["eCartorio:SP:UrlApi"]}/Certificate/all?city=São%20Paulo";

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync(urlGet);
                if (result.IsSuccessStatusCode)
                    return Ok();
                else
                    return BadRequest(new { result.StatusCode, content = await result.Content.ReadAsStringAsync() });
            }
        }

        [HttpGet("email")]
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

            var message = _smtp.SendEmail(email, "Olá, bem Vindo ao iFacilita!", body);


            return Ok(new { code = 200, message });
        }

        [HttpGet("emailApi")]
        public IActionResult SendMailApi([FromQuery] string email)
        {
            var body = string.Format("<h1>Olá Teste! {0}</h1> <br/> Seja bem vindo à ifacilita! <br/> <br/> Primeiramente gostaríamos de agradecer a preferência por nossa empresa. Muito obrigado! Faremos o possível para lhe oferecer um excelente atendimento.<br/> <br/> Seus dados cadastrais foram processados com sucesso e uma conta foi criada para você.<br/> <br/> Atenciosamente,<br/> ifacilita.com", DateTime.Now.ToLongDateString());

            _ = _clientEmailService.SendMail(
                new List<Tuple<string, string>>() { new Tuple<string, string>(email.Split('@')[0], email) }, "Olá! Teste de envio de email por API. à quem possa interessar, é teste", body,null, null);

            return Ok(new { code = 200, message = "" });
        }
    }
}
