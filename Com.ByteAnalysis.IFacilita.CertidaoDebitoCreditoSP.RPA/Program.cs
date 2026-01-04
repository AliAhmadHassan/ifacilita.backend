using Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.RPA.Captcha;
using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.CertidaoDebitoCreditoSP.RPA
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] TaxDebts - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] TaxDebts", "Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União");
            WriteLog("Iniciando serviço do RPA TaxDebts - Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União");
            WriteLog("Ambiente de execução: " + environmentName);

            IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW(Configuration["path:urlApi"]);
            IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW(Configuration["path:urlPut"]);

            IS3 s3 = new Common.Impl.S3();
            const int timeoutCicle = 1000 * 30;

            while (true)
            {
                WriteLog("Obtendo requisições pendentes");
                HttpResult<Model.CertidaoDebitoCreditoSP> get = httpClientFW.Get<Model.CertidaoDebitoCreditoSP>(new[] { "" });

                if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                {
                    WriteLog("Requisições pendentes foram encontradas. Iniciando processo do RPA");

                    Model.CertidaoDebitoCreditoSP certidao = get.Value;

                    WriteLog("Configurando driver do Chrome");

                    ChromeDriverService service = null;
                    ChromeOptions options = new ChromeOptions();

                    try
                    {
                        var pathPluginCaptcha = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                        WriteLog("Configurando plugin anti-captcha. " + pathPluginCaptcha);

                        options.AddArguments($"load-extension={pathPluginCaptcha}");
                        options.AddArguments("disable-infobars");

                        WriteLog("Configurando diretório do driver");

                        var currentVersion = Configuration["ChromeBinary:Version"];
                        var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
                        
                        service = ChromeDriverService.CreateDefaultService(pathDrive);
                        service.HideCommandPromptWindow = true;
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Não foi possível carregar os dados do driver." + ex.Message);
                        Thread.Sleep(timeoutCicle);
                        continue;
                    }

                    using (ChromeDriver chrome = new ChromeDriver(service, options))
                    {
                        WriteLog("Processo iniciado");
                        try
                        {
                            if (certidao.PessoaFisica)
                                chrome.Navigate().GoToUrl($"http://servicos.receita.fazenda.gov.br/Servicos/certidao/CndConjuntaInter/InformaNICertidao.asp?Tipo=2&ERR=parmacessoexpirado&NI={certidao.CpfCnpj}");
                            else
                                chrome.Navigate().GoToUrl($"http://servicos.receita.fazenda.gov.br/Servicos/certidao/CndConjuntaInter/InformaNICertidao.asp?Tipo=1&ERR=parmacessoexpirado&NI={certidao.CpfCnpj}");

                            chrome.Manage().Window.Maximize();

                            #region Quebrar o Captcha

                            var countIntent = 1;
                            var captchaSolved = false;
                            var captchaSolve = string.Empty;

                            do
                            {
                                captchaSolve = SolveCaptcha(chrome);

                                chrome.FindElement(By.Id("txtTexto_captcha_serpro_gov_br")).SendKeys(captchaSolve);
                                var btnSubimit = chrome.FindElement(By.Id("submit1"));
                                btnSubimit.Click();

                                var resultSubmit = AlertIsPresent(chrome);
                                if (string.IsNullOrEmpty(resultSubmit))
                                {
                                    captchaSolved = true;
                                    break;
                                }
                                else
                                {
                                    if (resultSubmit == "O número do CPF informado está incorreto")
                                    {
                                        certidao.Status = Common.Enumerable.APIStatus.Error;
                                        certidao.StatusProcess = Model.Status.ErrorOnProcessing;
                                        certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "cpfCnpj", Message = resultSubmit } };
                                        HttpResult<Model.CertidaoDebitoCreditoSP> responsePut = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                                        break;
                                    }
                                    else
                                    {
                                        WriteLog(resultSubmit);
                                    }
                                }

                                if (countIntent >= 5)
                                {
                                    certidao.Status = Common.Enumerable.APIStatus.Error;
                                    certidao.StatusProcess = Model.Status.ErrorOnProcessing;
                                    certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "Captcha", Message = "Não foi possível resolver o captcha da página" } };
                                }
                                else
                                {
                                    WriteLog($"Tentativa de resolução de Captcha: {countIntent.ToString("00")}");
                                    countIntent++;
                                }

                            } while (true);

                            if (!captchaSolved)
                                continue;

                            #endregion

                            bool loading = false;

                            Thread.Sleep(1000 * 2);


                            if (chrome.PageSource.Contains("As informações disponíveis na Secretaria da Receita Federal do Brasil - RFB"))
                            {
                                var messageValidation = "As informações disponíveis na Secretaria da Receita Federal do Brasil - RFB e na Procuradoria-Geral da Fazenda Nacional - PGFN sobre o contribuinte 009.434.857-03 são insuficientes para a emissão de certidão por meio da Internet. Para consultar sua situação fiscal, acesse Centro Virtual de Atendimento e-CAC.";
                                certidao.Status = Common.Enumerable.APIStatus.Error;
                                certidao.StatusProcess = Model.Status.ErrorOnProcessing;

                                certidao.Errors = new List<GlobalError>() { new GlobalError() {
                                    Code = 1,
                                    Field = "cpfCnpj",
                                    Message = messageValidation
                                }};

                                HttpResult<Model.CertidaoDebitoCreditoSP> responsePut = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                                continue;
                            }
                            else if (chrome.PageSource.Contains("Sistema temporariamente Indisponível. Tente novamente dentro de alguns minutos"))
                            {
                                certidao.Status = Common.Enumerable.APIStatus.Pending;
                                certidao.StatusProcess = Model.Status.Waiting;

                                HttpResult<Model.CertidaoDebitoCreditoSP> responsePut = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                                continue;
                            }

                            bool exists = chrome.FindElements(By.XPath("//a [text()='Emissão de nova certidão']")).Count > 0;

                            if (exists)
                            {
                                var btnNovaCertidao = WebDriverExtensions.WaitElementRender(chrome, By.XPath("//a [text()='Emissão de nova certidão']"));
                                btnNovaCertidao.Click();

                                if (chrome.PageSource.Contains("As informações disponíveis na Secretaria da Receita Federal do Brasil - RFB sobre o contribuinte 334.861.738-39 são insuficientes para a emissão de certidão por meio da Internet."))
                                {
                                    certidao.Status = Common.Enumerable.APIStatus.Error;
                                    certidao.StatusProcess = Model.Status.ErrorOnProcessing;
                                    certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "cpfCnpj", Message = "As informações disponíveis na Secretaria da Receita Federal do Brasil - RFB sobre o contribuinte 334.861.738-39 são insuficientes para a emissão de certidão por meio da Internet." } };
                                    HttpResult<Model.CertidaoDebitoCreditoSP> responsePut = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                                    continue;
                                }
                            }
                            else
                            {
                                if (chrome.PageSource.Contains("O número informado não consta do cadastro"))
                                {
                                    certidao.Status = Common.Enumerable.APIStatus.Error;
                                    certidao.StatusProcess = Model.Status.ErrorOnProcessing;
                                    certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "cpfCnpj", Message = "O número informado não consta do cadastro" } };
                                    HttpResult<Model.CertidaoDebitoCreditoSP> responsePut = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);
                                    continue;
                                } 
                            }

                            while (loading == false)
                            {
                                loading = chrome.FindElements(By.XPath("//a [text()='Nova Consulta']")).Count > 0;

                                if (loading)
                                    break;
                            }

                            #region salvar na S3
                            var doc = chrome.FindElement(By.Id("PRINCIPAL")).GetAttribute("innerHTML");

                            using (WebClient client = new WebClient())
                            {
                                Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("PRINCIPAL"))).GetScreenshot();
                                image.SaveAsFile($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg");

                                string baseContract64 = FileToBase64($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg").Result;
                                string keyS3 = s3.SaveFile(baseContract64, ".jpg");

                                certidao.IdDocS3 = $"https://ifacilita.s3.us-east-2.amazonaws.com/{keyS3}";
                            }
                            File.Delete($"CertidaoNegativaDebitosCreditos-{certidao.Nome}-{certidao.CpfCnpj}.jpg");
                            #endregion

                            #region Operações de callback para Api RPA e Api Core

                            Common.HttpResult<Model.CertidaoDebitoCreditoSP> postResult = httpClientFWPut.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);

                            if (postResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                            {
                                certidao.StatusModified = DateTime.Now;
                                certidao.Status = Common.Enumerable.APIStatus.Success;
                                certidao.StatusProcess = Model.Status.Finished;

                                if (!string.IsNullOrEmpty(certidao.UrlCallback))
                                {
                                    var callbackResult = new Common.Impl.HttpClientFW(certidao.UrlCallback).Post<object, object>(new[] { "" }, new { certidao.Id, orderId = "", certiticateType = "TaxDebts" });
                                    if (!(callbackResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult))
                                    {
                                        var responseCallback = (Microsoft.AspNetCore.Mvc.BadRequestObjectResult)callbackResult.ActionResult;
                                        var exception = (Exception)responseCallback.Value;

                                        certidao.StatusProcess = Model.Status.ErrorOnCallback;
                                        certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "UrlCallback", Message = exception.Message } };
                                    }
                                }
                                else
                                {
                                    certidao.StatusProcess = Model.Status.Finished;
                                    certidao.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "UrlCallback", Message = "Nenhuma URL de calback foi informada" } };
                                }
                            }

                            Common.IHttpClientFW httpCallback = new Common.Impl.HttpClientFW(Configuration["path:urlPut"]);
                            httpCallback.Put<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            certidao.StatusProcess = Model.Status.ErrorOnProcessing;
                            certidao.StatusModified = DateTime.Now;

                            httpClientFW.Post<Model.CertidaoDebitoCreditoSP, Model.CertidaoDebitoCreditoSP>(new[] { "" }, certidao);

                            Console.WriteLine($"Ocorreu um erro em um dos processos do ChomeDriver {ex.Message}");
                        }
                    }

                    WriteLog("Processo finalizado com sucesso");
                }
                else
                {
                    WriteLog("Nenhuma requisição pendente foi encontrada");
                }

                WriteLog("Aguardando próximo ciclo");

                Thread.Sleep(timeoutCicle);
            }
        }

        private static string SolveCaptcha(ChromeDriver driver)
        {
            if (!Directory.Exists("download-captcha"))
                Directory.CreateDirectory("download-captcha");

            Screenshot image = ((ITakesScreenshot)driver.FindElement(By.Id("imgCaptchaSerpro"))).GetScreenshot();

            var fileName = Path.Combine(Environment.CurrentDirectory, "download-captcha", Guid.NewGuid().ToString("N") + ".jpg");
            image.SaveAsFile(fileName);

            var captchaSolve = AntiCaptchaClient.CaptchaSolve(fileName).Result;

            return captchaSolve;
        }

        private static async Task<string> FileToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }

        private static void WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/TaxDebts-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch { }
        }

        private static void WriteLogStart(string key)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/ifacilitaStart-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{key}] - TaxDebts ->  Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch { }
        }

        private static string AlertIsPresent(ChromeDriver driver, int timeout = 3)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                string message = string.Empty;
                if (alert != null)
                {
                    message = alert.Text;
                    alert.Accept();
                }

                return message;
            }
            catch (NoAlertPresentException aEx)
            {
                return string.Empty;
            }
        }

        private static bool LoadCompleted(ChromeDriver driver, int timeout = 3)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeout));
                return wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
            }
            catch { return false; }
        }
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by)
        {
            int timeoutInSeconds = 5;
            int tentativa = 5;

            if (timeoutInSeconds > 0)
            {

                while (tentativa > 0)
                {
                    try
                    {
                        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                        return wait.Until(drv => drv.FindElement(by));
                    }
                    catch
                    {
                        Console.WriteLine($"Não foi encotrado do elemento -> {by.ToString()}");
                    }

                    tentativa--;
                }
            }

            return driver.FindElement(by);

        }

        public static IWebElement WaitElementRender(this IWebDriver driver, By by)
        {
            int timer = 3000;

            while (true)
            {
                if (driver.FindElements(by).Count > 0)
                {
                    System.Threading.Thread.Sleep(timer * 2);
                    return driver.FindElement(by);
                }

                Console.WriteLine($"Localizando input com o nome: {by.ToString()}");
                System.Threading.Thread.Sleep(timer);
            }

        }
    }

}
