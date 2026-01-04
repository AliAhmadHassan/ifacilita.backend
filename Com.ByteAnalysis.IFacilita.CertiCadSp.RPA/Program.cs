using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.CertiCadSp.RPA
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] PropertyRegistrationData - Dados Cadastrais do Imóvel";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] PropertyRegistrationData", "Dados Cadastrais do Imóvel");
            WriteLog("Iniciando serviço do RPA PropertyRegistrationData - Dados Cadastrais do Imóvel");
            WriteLog("Ambiente de execução: " + environmentName);

            string user = Configuration["user:email"];
            string password = Configuration["user:password"];

            IS3 s3 = new Common.Impl.S3();
            IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW(Configuration["Api:Get"]);
            IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW(Configuration["Api:Put"]);


            List<GlobalError> erros = new List<GlobalError>();
            while (true)
            {
                WriteLog("Obtendo requisições pendentes");

                Common.HttpResult<Model.Requisition> get = httpClientFW.Get<Model.Requisition>(new[] { "" });

                if (get.ActionResult is OkResult)
                {
                    erros = new List<GlobalError>();

                    Model.Requisition requisition = get.Value;
                    WriteLog("Requisições pedentes foram encontradas. Iniciando o processamento. Id: " + requisition.Id);
                    
                    if (requisition.Errors != null)
                        erros = requisition.Errors.ToList();

                    const int timeoutCicle = 1000 * 30;

                    ChromeDriverService service = null;
                    ChromeOptions options = new ChromeOptions();

                    var downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
                    if (!Directory.Exists(downloadDirectory))
                        Directory.CreateDirectory(downloadDirectory);

                    try
                    {
                        var pathPluginCaptcha = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                        WriteLog("Configurando plugin anti-captcha. " + pathPluginCaptcha);

                        options.AddArguments($"load-extension={pathPluginCaptcha}");
                        options.AddArguments("disable-infobars");
                        options.AddUserProfilePreference("download.default_directory", downloadDirectory);
                        options.AddUserProfilePreference("disable-popup-blocking", "true");

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
                        const int timer = 2000;

                        try
                        {
                            chrome.Navigate().GoToUrl(Configuration["PropertyRegistrationData:Url"]);
                            chrome.Manage().Window.Maximize();

                            chrome.FindElement(By.Id("formBody_txtUser")).SendKeys(requisition.CpfCnPj);

                            chrome.FindElement(By.Id("formBody_txtPassword")).SendKeys(requisition.Password);
                            Thread.Sleep(timer);

                            #region captchSolver
                            string imagem = chrome.FindElement(By.Id("imgCaptcha")).GetAttribute("src");

                            using (WebClient client = new WebClient())
                            {
                                Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("imgCaptcha"))).GetScreenshot();
                                string imgValue = CaptchaSolveWithBase64(image.AsBase64EncodedString).Result;
                                chrome.FindElement(By.Id("txtValidacao")).SendKeys(imgValue);
                            }

                            #endregion

                            //validar Cpf-Cnpj
                            try
                            {
                                var validationDocument = chrome.FindElement(By.Id("formBody_CustomValidator1"));
                                var msgValidation = validationDocument.Text;

                                if (!string.IsNullOrEmpty(msgValidation))
                                {
                                    requisition.Status = Common.Enumerable.APIStatus.Error;
                                    requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                                    requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "cpfCnPj", Message = msgValidation } };

                                    httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                    WriteLog("Houve falha na validação do documento. " + msgValidation);
                                    continue;
                                }
                                else
                                {
                                    WriteLog("Valor e do documento correto.");
                                }
                            }
                            catch
                            {
                                WriteLog("Valor e do documento correto.");
                            }

                            System.Threading.Thread.Sleep(timer);

                            var formBody_Button1 = chrome.FindElement(By.Id("formBody_Button1"));
                            formBody_Button1.Click();

                            try
                            {
                                var validationRequest = chrome.FindElement(By.Id("formBody_UserPasswordCustomValidator"));
                                var msgRequest = validationRequest.Text;
                                if (!string.IsNullOrEmpty(msgRequest))
                                {
                                    requisition.Status = Common.Enumerable.APIStatus.Error;
                                    requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                                    requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "cpfCnPj", Message = msgRequest } };

                                    httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                    WriteLog(msgRequest);
                                    continue;
                                }
                            }
                            catch
                            {
                                WriteLog("Dados de acesso estão corretos");
                            }

                            if (string.IsNullOrEmpty(requisition.Sql))
                            {
                                WriteLog("O campo SQL(Cadastro do Imóvel) não foi informado.");
                                erros.Add(new GlobalError() { Field = "Sql", Code = 3, Message = "O campo SQL(Cadastro do Imóvel) não foi informado." });
                                requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                                requisition.StatusModified = DateTime.Now;
                                requisition.Status = Common.Enumerable.APIStatus.Error;

                                requisition.Errors = erros;

                                httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                continue;
                            }

                            chrome.FindElement(By.Id("txt_SQL")).SendKeys(requisition.Sql);
                            Thread.Sleep(timer);

                            chrome.FindElement(By.Id("txt_Exercicio")).SendKeys(requisition.dateDoc);
                            Thread.Sleep(timer);
                            var btnConsultar = chrome.FindElement(By.Id("btnConsultar"));
                            btnConsultar.Click();

                            try
                            {
                                var sqlValidation = chrome.FindElement(By.Id("popup_message"));
                                var msgSqlValidation = sqlValidation.Text;

                                if (!string.IsNullOrEmpty(msgSqlValidation))
                                {
                                    requisition.Status = Common.Enumerable.APIStatus.Error;
                                    requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                                    requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 3, Field = "sql", Message = msgSqlValidation } };

                                    httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                    WriteLog(msgSqlValidation);
                                    continue;
                                }
                            }
                            catch
                            {
                                WriteLog("Sql Ok");
                            }

                            Thread.Sleep(5000);
                            #region Localizando e salvando pdf no S3
                            DirectoryInfo directoryInfo = new DirectoryInfo(downloadDirectory);

                            string fileName = "";
                            foreach (FileInfo file in directoryInfo.GetFiles())
                            {
                                if (file.Extension.Contains(".pdf"))
                                {
                                    downloadDirectory = file.FullName;
                                    fileName = file.Name;
                                    break;
                                }
                            }

                            using (WebClient client = new WebClient())
                            {
                                string baseContract64 = FileToBase64(downloadDirectory).Result;
                                string keyS3 = s3.SaveFile(baseContract64, ".pdf");
                                requisition.s3patch = $"https://ifacilita.s3.us-east-2.amazonaws.com/{keyS3}";
                                requisition.UrlCertification = $"https://ifacilita.s3.us-east-2.amazonaws.com/{keyS3}";
                                Thread.Sleep(3000);
                                File.Delete(downloadDirectory);
                            }

                            #endregion

                            #region Operações de callback para Api RPA e Api Core

                            HttpResult<Model.Requisition> postResult = httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                            Thread.Sleep(3000);

                            if (postResult.ActionResult is OkResult)
                            {
                                requisition.StatusModified = DateTime.Now;
                                requisition.StatusProcess = Model.Status.Finished;
                                requisition.Status = Common.Enumerable.APIStatus.Success;

                                requisition.Expiration = DateTime.Now.AddMonths(3).AddDays(-1);
                                requisition.Received = DateTime.Now;
                                requisition.CallbackResponse = DateTime.Now;

                                if (!string.IsNullOrEmpty(requisition.UrlCallback))
                                {
                                    var callbackResult = new Common.Impl.HttpClientFW(requisition.UrlCallback).Post<object, object>(new[] { "" }, new { requisition.Id, orderId = "", certiticateType = "PropertyRegistrationData" });
                                    if (!(callbackResult.ActionResult is OkResult))
                                    {
                                        var responseCallback = (Microsoft.AspNetCore.Mvc.BadRequestObjectResult)callbackResult.ActionResult;
                                        var exception = (Exception)responseCallback.Value;

                                        requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                        erros.Add(new GlobalError() { Code = 0, Field = "UrlCallback", Message = exception.Message });
                                        requisition.Errors = erros;
                                    }
                                }
                                else
                                {
                                    requisition.StatusProcess = Model.Status.Finished;
                                    erros.Add(new GlobalError() { Code = 0, Field = "UrlCallback", Message = "Nenhuma URL de callback foi informada" });
                                    requisition.Errors = erros;
                                }

                            }
                            else
                            {
                                var response = (BadRequestObjectResult)postResult.ActionResult;
                                var exceptionException = (Exception)response.Value;

                                requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                erros.Add(new GlobalError() { Code = 0, Field = "Put", Message = exceptionException.Message });
                                requisition.Errors = erros;
                            }

                            WriteLog("Atualizando dados da requisição Id: " + requisition.Id);
                            httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                            WriteLog("Processo finalizado com sucesso");

                            #endregion
                        }
                        catch (Exception ex)
                        {

                            requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                            requisition.StatusModified = DateTime.Now;

                            erros.Add(new GlobalError() { Code = 500, Field = "General", Message = ex.Message });
                            requisition.Errors = erros;

                            httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                            Console.WriteLine($"Ocorreu um erro em um dos processos do ChomeDriver {ex.Message}");

                        }
                    }
                }
                else
                {
                    WriteLog("Nenhuma requisição pendente para processamento");
                }

                WriteLog("Aguardando próximo ciclo de processamento");
                Thread.Sleep(30 * 1000);
            }
        }

        private static void WriteLog(string msg)
        {
            try
            {
                var strMessage = $"[ {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {msg}";
                Console.WriteLine(strMessage);

                File.AppendAllLines($"PropertyRegistrationData.{DateTime.Now.ToString("dd-MM-yyyy-HH")}.log", new[] { strMessage });
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Falha ao tentar gerar Logs. " + ex.Message);
            }
        }

        private static void WriteLogStart(string key)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["WriteLog:Path"]}/ifacilitaStart-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{key}] - PropertyRegistrationData - Dados Cadastrais do Imóvel";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tenta gravar log de inicialização. " + ex.Message);
            }
        }

        public static async Task<string> CaptchaSolveWithBase64(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(pathImage);

            return image.Response;
        }

        private static async Task<string> FileToBase64(string path)
        {
            if (!path.Contains("CertidaoDadosCad.pdf"))
            {
                path = path + "\\CertidaoDadosCad.pdf";
            }

            byte[] imageArray = File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
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