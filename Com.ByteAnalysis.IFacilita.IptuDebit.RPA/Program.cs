using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.RPA
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] IptuDebts -  Débitos do IPTU";

            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] IptuDebts", "Débitos do IPTU");

            WriteLog("Iniciando RPA Débitos do IPTU");
            WriteLog("Ambiente de execução: " + environmentName);

            string urlApiBase = Configuration["Api:UrlBase"];

            Common.IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW($"{urlApiBase}/{Configuration["Api:Get"]}");
            Common.IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW($"{urlApiBase}/{Configuration["Api:Put"]}");

            while (true)
            {
                try
                {
                    WriteLog("Procurando requisições pendentes");
                    Common.HttpResult<Model.Requisition> get = httpClientFW.Get<Model.Requisition>(new[] { "" });

                    //Model.Requisition requisition = get.
                    if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                    {
                        WriteLog("Requisição encontrada");
                        WriteLog("Configurando driver do Chrome");
                        ChromeOptions options = new ChromeOptions();

                        var pathPluginCaptcha = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");

                        WriteLog("Configurando plugin anti-captcha. " + pathPluginCaptcha);

                        options.AddArguments($"load-extension={pathPluginCaptcha}");
                        options.AddArguments("disable-infobars");

                        WriteLog("Configurando diretório do driver");

                        var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
                        var service = ChromeDriverService.CreateDefaultService(pathDrive);

                        service.HideCommandPromptWindow = true;

                        Model.Requisition requisition = get.Value;

                        using (ChromeDriver chrome = new ChromeDriver(service, options))
                        {
                            chrome.Manage().Window.Maximize();
                            chrome.Navigate().GoToUrl(Configuration["Urls:Base"]);

                            var selectElement = new SelectElement(chrome.FindElementById("ctl00_ConteudoPrincipal_ddlTipoCertidao"));
                            selectElement.SelectByValue("2");

                            var imageBase64 = chrome.ExecuteScript(@"
                                var c = document.createElement('canvas');
                                var ctx = c.getContext('2d');
                                var img = document.getElementById('ctl00_ConteudoPrincipal_imgCaptcha');
                                c.height=img.naturalHeight;
                                c.width=img.naturalWidth;
                                ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
                                var base64String = c.toDataURL();
                                return base64String;
                            ") as string;

                            string solved = CaptchaSolveWithBase64(imageBase64.Remove(0, imageBase64.IndexOf(",") + 1)).Result;
                            chrome.FindElementById("ctl00_ConteudoPrincipal_txtValorCaptcha").SendKeys(solved);

                            chrome.FindElementById("ctl00_ConteudoPrincipal_txtSQL").Clear();
                            foreach (char character in requisition.SQL)
                                chrome.FindElementById("ctl00_ConteudoPrincipal_txtSQL").SendKeys(character.ToString());

                            System.Threading.Thread.Sleep(2000);
                            chrome.FindElementById("ctl00_ConteudoPrincipal_btnEmitir").Click();

                            var messageClick = AlertIsPresent(chrome);
                            if (!string.IsNullOrEmpty(messageClick))
                            {
                                messageClick = messageClick.Replace("\r", "").Replace("\n", "");
                                if (messageClick.Contains("Documento. Setor Quadra Lote informado incorretamente"))
                                {
                                    requisition.Status = Common.Enumerable.APIStatus.Error;
                                    requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                    requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "sql", Message = messageClick } };
                                    Common.HttpResult<Model.Requisition> responsePut = httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                    continue;
                                }
                            }

                            string file = Common.Impl.HttpClientFW.WaitingDownloaded("Relatorio_Certidao_Imob_");

                            Common.IS3 s3 = new Common.Impl.S3();
                            requisition.UrlCertification = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(FileToBase64(file).Result, ".pdf");
                            requisition.Received = DateTime.Now;
                            requisition.Expiration = DateTime.Today.AddMonths(3).AddDays(-1);

                            File.Delete(file);

                            Common.HttpResult<Model.Requisition> postResult = httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                            if (postResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                            {
                                requisition.StatusModified = DateTime.Now;
                                requisition.Status = Common.Enumerable.APIStatus.Success;
                                requisition.StatusProcess = Model.Status.Finished;

                                if (!string.IsNullOrEmpty(requisition.UrlCallback))
                                {
                                    var callbackResult = new Common.Impl.HttpClientFW(requisition.UrlCallback).Post<object, object>(new[] { "" }, new { requisition.Id, orderId = "", certiticateType = "IptuDebts" });

                                    if (!(callbackResult.ActionResult is Microsoft.AspNetCore.Mvc.OkResult))
                                    {
                                        var responseCallback = (Microsoft.AspNetCore.Mvc.BadRequestObjectResult)callbackResult.ActionResult;
                                        var exception = (Exception)responseCallback.Value;

                                        requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                        requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "UrlCallback", Message = exception.Message } };
                                    }
                                }
                                else
                                {
                                    requisition.StatusProcess = Model.Status.Finished;
                                    requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "UrlCallback", Message = "Nenhuma URL de callback foi informada" } };
                                }
                            }
                            else
                            {
                                var response = (Microsoft.AspNetCore.Mvc.BadRequestObjectResult)postResult.ActionResult;
                                var exceptionException = (Exception)response.Value;

                                requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "Put", Message = exceptionException.Message } };
                            }

                            WriteLog($"Atualizando status da requisição: {requisition.Id}");
                            httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                            WriteLog($"Requisição: {requisition.Id} atualizada com sucesso");
                        }
                    }
                    else
                    {
                        WriteLog("Nenhuma requisição pendnete");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message);
                }

                WriteLog("Aguardando próximo ciclo de processamento");
                System.Threading.Thread.Sleep(30 * 1000);
            }
        }

        private static void WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/IptuDebit-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

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

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{key}] - IptuDebit ->  Débitos do IPTU";
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
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }
    }
}
