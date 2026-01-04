using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Naturgy.Model;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {

        bool encontrou = false;

        private Random randomPassword = new Random();

        Service.IRegisterClientService registerClientService;

        public RegisterController(Service.IRegisterClientService registerClientService)
        {
            this.registerClientService = registerClientService;
        }

        [HttpGet]
        public IActionResult GetAllRequisition()
        {
            try
            {
                List<RegisterClient> requisitions = registerClientService.Get();
                return Ok(requisitions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            try
            {
                var result = registerClientService.Get(id);
                if (result == null)
                    return NotFound(new { message = "Requisition not found" });

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] RegisterClient requisition)
        {
            return StatusCode(201, this.registerClientService.CreateOrUpdate(requisition));
        }

        [HttpPut]
        public ActionResult Put([FromBody] Model.RegisterClient requisition)
        {
            return Ok(this.registerClientService.CreateOrUpdate(requisition));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            try
            {
                registerClientService.Remove(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("changeTitulary")]
        public IActionResult loginNaturgy([FromBody] Model.RegisterClient client)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                var pathPlugin = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugin", "anticaptcha-plugin_v0.50");
                options.AddArguments($"load-extension={pathPlugin}");
                options.AddArguments("disable-infobars");

                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability(ChromeOptions.Capability, options);


                List<GlobalError> errosNaturgy = new List<GlobalError>();

                using (ChromeDriver chrome = new ChromeDriver(options))
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;

                    chrome.Manage().Window.Maximize();
                    chrome.Navigate().GoToUrl("https://portal.minhanaturgy.com.br/");

                    chrome.FindElement(By.Id("btnLogin")).Click();
                    Thread.Sleep(7000);

                    chrome.FindElement(By.Id("CpfCnpj")).SendKeys(client.CpfCnpj);
                    Thread.Sleep(1000);

                    chrome.FindElement(By.Id("Senha")).SendKeys(client.Password);
                    Thread.Sleep(1000);

                    chrome.FindElement(By.Id("btnEntrar")).Click();

                    try
                    {
                        var cpfCnpjInvalid = chrome.FindElement(By.Id("CpfCnpj-error"));
                        errosNaturgy.Add(new GlobalError() { Code = 1, Field = "socialSecurityNumber", Message = cpfCnpjInvalid.Text });
                        Thread.Sleep(250);
                    }
                    catch { }

                    var countIntent = 0;
                    var solved = true;

                    try
                    {
                        while (chrome.FindElement(By.TagName("body")).FindElement(By.ClassName("antigate_solver")).Text != "Solved")
                        {
                            if (countIntent >= (60 * 5))
                            {
                                solved = false;
                                break;
                            }
                            countIntent++;
                            Thread.Sleep(1000);
                            Console.WriteLine("Aguardando resolução do captcha. " + (countIntent + 1) + "s");
                        }
                    }
                    catch (Exception ex)
                    {
                        solved = true;
                    }

                    if (!solved)
                    {
                        errosNaturgy.Add(new GlobalError() { Code = 0, Field = "General", Message = "Naturgy está solicitando a resolução do captcha de reconhecimento de imagens" });
                        client.Errors = errosNaturgy;
                        client.Status = Common.Enumerable.APIStatus.Error;
                        client.StatusProccess = Status.ErrorOnProcessing;
                        registerClientService.CreateOrUpdate(client);
                        return Ok(client);
                    }


                    if (errosNaturgy.Count() > 0)
                    {
                        client.Errors = errosNaturgy;
                        client.Status = Common.Enumerable.APIStatus.Error;
                        client.StatusProccess = Status.ErrorOnProcessing;
                        registerClientService.CreateOrUpdate(client);
                        return Ok(client);
                    }


                    while (true)
                    {
                        string text = chrome.FindElement(By.CssSelector(".dp-flex > .section-title")).Text;
                        if (text == "CLIENTE")
                        {
                            encontrou = true;
                            break;
                        }
                    }

                    Thread.Sleep(3000);
                    chrome.FindElement(By.CssSelector(".menu-item > .icon-SolicitarGas")).Click();
                    Thread.Sleep(3000);
                    chrome.FindElement(By.Id("CEP")).Click();
                    chrome.FindElement(By.Id("CEP")).SendKeys(client.Address.Cep.ToString());
                    Thread.Sleep(3000);

                    try
                    {
                        var msgPopUp = chrome.FindElement(By.Id("modalMensagem"));
                        var msgElement = msgPopUp.FindElement(By.ClassName("message"));
                        var msgText = msgElement.Text;

                        if (msgText == "Atenção! Não localizamos o CEP informado. Por favor preencha os campos manualmente para prosseguir com a sua solicitação.")
                        {
                            errosNaturgy.Add(new GlobalError() { Code = 2, Field = "Address.zipCode", Message = msgText });
                            chrome.FindElement(By.Id("id_btn_fechar_solicitacao_gas")).Click();
                            Thread.Sleep(1000);
                        }

                    }
                    catch { }

                    try
                    {
                        var cepInvalid = chrome.FindElement(By.Id("CEP-error"));
                        errosNaturgy.Add(new GlobalError() { Code = 2, Field = "Address.zipCode", Message = cepInvalid.Text });
                        Thread.Sleep(250);
                    }
                    catch { }

                    chrome.FindElement(By.CssSelector(".row > .ctn-checkbox .checkmark")).Click();
                    Thread.Sleep(500);

                    try
                    {
                        chrome.FindElement(By.Id("id_btn_fechar_solicitacao_gas")).Click();
                        Thread.Sleep(500);
                    }
                    catch { }

                    Thread.Sleep(3000);
                    chrome.FindElement(By.Id("NumEndereco")).Click();
                    chrome.FindElement(By.Id("NumEndereco")).SendKeys(client.Address.Number.ToString());
                    Thread.Sleep(250);

                    try
                    {
                        var numInvalid = chrome.FindElement(By.Id("NumEndereco-error"));
                        errosNaturgy.Add(new GlobalError() { Code = 3, Field = "Address.number", Message = numInvalid.Text });
                        Thread.Sleep(250);
                    }
                    catch { }
                    finally { Thread.Sleep(2750); }

                    if (errosNaturgy.Count() > 0)
                    {
                        client.Errors = errosNaturgy;
                        client.Status = Common.Enumerable.APIStatus.Error;
                        client.StatusProccess = Status.ErrorOnProcessing;
                        registerClientService.CreateOrUpdate(client);
                        return Ok(client);
                    }

                    chrome.FindElement(By.Id("btnFinalizar")).Click();
                    Thread.Sleep(3000);
                    while (true)
                    {
                        string text = chrome.FindElement(By.Id("id_btn_confirmar_solicitacao_gas")).Text;
                        if (text == "Confirmar e finalizar")
                        {
                            encontrou = true;
                            break;
                        }

                        Thread.Sleep(1000);
                    }

                    chrome.FindElement(By.Id("id_btn_confirmar_solicitacao_gas")).Click();
                    Thread.Sleep(3000);
                    chrome.FindElement(By.CssSelector(".numProtocolo:nth-child(2)")).Click();
                    chrome.FindElement(By.CssSelector(".numProtocolo:nth-child(2)")).Click();
                }

                client.StatusProccess = Status.Finished;
                client.StatusModified = DateTime.Now;
                client.Status = Common.Enumerable.APIStatus.Success;
                return Ok(this.registerClientService.CreateOrUpdate(client));
            }
            catch (Exception ex)
            {
                client.StatusProccess = Status.ErrorOnProcessing;
                client.StatusModified = DateTime.Now;

                this.registerClientService.CreateOrUpdate(client);

                return StatusCode(500, new { message = ex.Message, description = "Erro em troca de titulariedade", timestamp = DateTime.Now });
            }
        }

        [HttpPost("registerNaturgy")]
        public IActionResult registerNaturgy([FromBody] Model.RegisterClient client)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                var pathPlugin = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugin", "anticaptcha-plugin_v0.50");
                options.AddArguments($"load-extension={pathPlugin}");
                options.AddArguments("disable-infobars");

                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability(ChromeOptions.Capability, options);

                var timeout = 1000;
                using (ChromeDriver chrome = new ChromeDriver(options))
                {
                    client.Password = this.randomPassword.GetHashCode().ToString();

                    chrome.Manage().Window.Maximize();
                    chrome.Navigate().GoToUrl("https://portal.minhanaturgy.com.br/Login/Registrar");
                    chrome.FindElement(By.Id("CpfCnpj")).SendKeys(client.CpfCnpj);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("NomeCompleto")).SendKeys(client.FullName);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("Email")).SendKeys(client.Email);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("ConfirmarEmail")).SendKeys(client.Email);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("Senha")).SendKeys(client.Password);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("ConfrimarSenha")).SendKeys(client.Password);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.Id("Celular")).SendKeys("21" + client.CellPhone);
                    Thread.Sleep(timeout);
                    chrome.FindElement(By.CssSelector(".checkmark")).Click();
                    Thread.Sleep(timeout);

                    var errors = chrome.FindElements(By.ClassName("text-danger"));
                    var fieldNumber = 0;

                    List<GlobalError> globalErrors = new List<GlobalError>();

                    foreach (var validate in errors)
                    {
                        try
                        {
                            var span = validate.FindElement(By.TagName("span"));
                            var msgerror = span.Text;

                            var field = "General";

                            switch (msgerror)
                            {
                                case "O campo \"CPF / CNPJ\" é obrigatório.": fieldNumber = 1; field = "socialSecurityNumber"; break;
                                case "O campo \"Nome Completo\" é obrigatório.": fieldNumber = 2; field = "name"; break;
                                case "O campo \"E - mail\" é obrigatório.": fieldNumber = 3; field = "eMail"; break;
                                case "O campo \"Confirmar E-mail\" é obrigatório.": fieldNumber = 4; field = "eMail"; break;
                                case "A senha deve conter letras e números.": fieldNumber = 5; field = "Password"; break;
                                case "O campo \"Confirmar Senha\" é obrigatório.": fieldNumber = 6; field = "Password"; break;
                                case "Número de celular inválido.": fieldNumber = 7; field = "mobileNumber"; break;
                                default:
                                    break;
                            }

                            globalErrors.Add(new GlobalError() { Code = fieldNumber, Field = field, Message = msgerror });
                        }
                        catch { }
                    }

                    if (globalErrors.Count > 0)
                    {
                        client.Errors = globalErrors;
                        client.Status = Common.Enumerable.APIStatus.Error;
                        client.StatusProccess = Status.ErrorOnProcessing;
                        registerClientService.CreateOrUpdate(client);
                        return Ok(client);
                    }

                    try
                    {
                        chrome.FindElement(By.Id("btnRegistrar")).Click();
                        Thread.Sleep(4000);
                    }
                    catch { }

                    var countIntent = 0;
                    var solved = true;

                    while (chrome.FindElement(By.TagName("body")).FindElement(By.ClassName("antigate_solver")).Text != "Solved")
                    {
                        if (countIntent >= (60 * 5))
                        {
                            solved = false;
                            break;
                        }
                        countIntent++;
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine("Aguardando resolução do captcha. " + (countIntent + 1) + "s");
                    }

                    if (!solved)
                    {
                        globalErrors.Add(new GlobalError() { Code = 0, Field = "General", Message = "Naturgy está solicitando a resolução do captcha de reconhecimento de imagens" });
                        client.Errors = globalErrors;
                        client.Status = Common.Enumerable.APIStatus.Error;
                        client.StatusProccess = Status.ErrorOnProcessing;
                        registerClientService.CreateOrUpdate(client);
                        return Ok(client);
                    }

                    while (true)
                    {
                        string text = chrome.FindElement(By.CssSelector(".dp-flex > .section-title")).Text;
                        if (text == "CLIENTE")
                        {
                            encontrou = true;
                            break;
                        }
                    }
                }

                client.Status = Common.Enumerable.APIStatus.Success;
                client.StatusProccess = Status.Finished;
                return Ok(this.registerClientService.CreateOrUpdate(client));
            }
            catch (Exception ex)
            {
                client.StatusProccess = Status.ErrorOnProcessing;
                client.StatusModified = DateTime.Now;
                client.Status = Common.Enumerable.APIStatus.Error;
                client.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "General", Message = ex.Message } };
                this.registerClientService.CreateOrUpdate(client);

                return StatusCode(500, new { message = ex.Message, description = "Erro em troca de titulariedade", timestamp = DateTime.Now });
            }
        }
    }
}