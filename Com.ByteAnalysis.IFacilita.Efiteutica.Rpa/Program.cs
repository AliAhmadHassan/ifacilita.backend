using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Com.ByteAnalysis.IFacilita.Efiteutica.Model;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Rpa
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static ChromeDriver _webDriver;

        private static async Task WriteLog(string message)
        {
            var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";

            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/Efiteutica-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                await File.AppendAllLinesAsync(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch
            {
                await Console.Out.WriteLineAsync($"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - Não foi possível gravar no arquivo de logs");
                await Console.Out.WriteLineAsync(msg);
            }
        }

        private static async Task<string> FileToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }

        public static async Task<string> CaptchaSolveWithBase64(string pathImage)
        {
            var captcha = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92");
            var captchaCustomHttp = new AntiCaptchaAPI.AntiCaptcha("08fa42956bda7a3e3896ccba6b454c92", new System.Net.Http.HttpClient());

            var balance = await captcha.GetBalance();

            var image = await captcha.SolveImage(pathImage);

            return image.Response;
        }

        private static async Task<string> AlertIsPresent(int timeout = 3)
        {
            try
            {
                var alert = _webDriver.SwitchTo().Alert();
                string message = null;
                if (alert != null)
                {
                    message = alert.Text;
                    alert.Accept();
                }

                return await Task.FromResult(message);
            }
            catch (NoAlertPresentException aEx)
            {
                return await Task.FromResult("");
            }
        }

        static async Task Main(string[] args)
        {
            try
            {
                Console.Title = "[RPA] Efiteutica - Certidão de Situação Fiscal e Enfitêutica do Imóvel";
                var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
                var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

                Configuration = new ConfigurationBuilder()
                      .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddCommandLine(args)
                      .Build();

                await WriteLog("Chave de Inicialização: " + envStart);
                await WriteLog("Iniciando Efiteutica - Certidão de Situação Fiscal e Enfitêutica do Imóvel");
                await WriteLog("Ambiente de execução: " + environmentName);

                LocalLog.WriteLogStart(Configuration, "[RPA] Efiteurica", "Certidão de Situação Fiscal e Enfitêutica do Imóvel");

                Common.IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW(Configuration["Api:Get"]);
                Common.IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW(Configuration["Api:Put"]);

                int timeoutElement = 1000;
                var cultureInfo = new CultureInfo("pt-BR");
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                List<GlobalError> errors = null;

                while (true)
                {
                    try
                    {
                        errors = new List<GlobalError>();
                        Common.HttpResult<IEnumerable<RequisitionModel>> get = httpClientFW.Get<IEnumerable<RequisitionModel>>(new[] { "" });

                        if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                        {
                            await WriteLog("Existem requisições pendentes de processamento.");

                            foreach (var item in get.Value)
                            {
                                RequisitionModel requisition = item;

                                await WriteLog("Iniciando processamento da requisição: " + requisition.Id);
                                ChromeDriverService service = null;
                                ChromeOptions options = new ChromeOptions();
                                var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");

                                //Carregar Plugins
                                try
                                {
                                    if (!Directory.Exists(dirDownload))
                                        Directory.CreateDirectory(dirDownload);

                                    var pathPluginCaptcha = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                                    var pathPluginPrint = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "scrnli");

                                    await WriteLog("Configurando plugin anti-captcha.");
                                    await WriteLog("Configurando plugin ScRn.");

                                    options.AddUserProfilePreference("download.default_directory", dirDownload);
                                    options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                                    options.AddUserProfilePreference("download.prompt_for_download", false);
                                    options.AddUserProfilePreference("download.directory_upgrade", true);
                                    options.AddUserProfilePreference("safebrowsing_for_trusted_sources_enabled", false);
                                    options.AddUserProfilePreference("safebrowsing.enabled", false);

                                    options.AddArguments($"load-extension={pathPluginCaptcha}");
                                    options.AddArguments($"load-extension={pathPluginPrint}");
                                    options.AddArguments("--disable-javascript");

                                    options.AddArguments("disable-infobars");

                                    await WriteLog("Configurando diretório do driver");

                                    var currentVersion = Configuration["ChromeBinary:Version"];

                                    var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
                                    service = ChromeDriverService.CreateDefaultService(pathDrive);

                                    service.HideCommandPromptWindow = true;
                                }
                                catch (Exception ex)
                                {
                                    await WriteLog("Não foi possível carregar os dados do driver." + ex.Message);
                                    continue;
                                }

                                string currentWindowsHandler = string.Empty;

                                using (ChromeDriver chrome = new ChromeDriver(service, options))
                                {
                                    _webDriver = chrome;

                                    System.Threading.Thread.Sleep(timeoutElement * 3);
                                    chrome.SwitchTo().Window(chrome.WindowHandles[1]).Close();
                                    chrome.SwitchTo().Window(chrome.WindowHandles[0]);

                                    chrome.Manage().Window.Maximize();
                                    chrome.Navigate().GoToUrl(Configuration["Urls:Site"]);
                                    currentWindowsHandler = chrome.CurrentWindowHandle;

                                    var hasError = false;
                                    var currentPage = false;

                                    while (chrome.PageSource.Contains("Nesta página, você poderá imprimir a Certidão de Situação Fiscal e Enfitêutica"))
                                    {
                                        if (hasError)
                                            break;

                                        chrome.FindElementByName("inscricao").Clear();
                                        chrome.FindElementByName("inscricao").SendKeys(requisition.IptuNumber.ToString());

                                        System.Threading.Thread.Sleep(timeoutElement);

                                        string imagem = chrome.FindElement(By.Id("img")).GetAttribute("src");
                                        using (WebClient client = new WebClient())
                                        {
                                            Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("img"))).GetScreenshot();
                                            string imgValue = await CaptchaSolveWithBase64(image.AsBase64EncodedString);
                                            chrome.FindElement(By.Id("texto_imagem")).SendKeys(imgValue);
                                            System.Threading.Thread.Sleep(timeoutElement);
                                        }

                                        chrome.FindElement(By.XPath("//input[@value='Consultar']")).Click();
                                        System.Threading.Thread.Sleep(timeoutElement);

                                        var messageAlert = await AlertIsPresent();

                                        while (true)
                                        {
                                            switch (messageAlert)
                                            {
                                                case "Código digitado não confere! Favor refazer a consulta!":
                                                case "CÓDIGO digitado Inválido!":
                                                    using (WebClient client = new WebClient())
                                                    {
                                                        Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("img"))).GetScreenshot();
                                                        string imgValue = await CaptchaSolveWithBase64(image.AsBase64EncodedString);
                                                        chrome.FindElement(By.Id("texto_imagem")).SendKeys(imgValue);
                                                        System.Threading.Thread.Sleep(timeoutElement);
                                                        chrome.FindElement(By.XPath("//input[@value='Consultar']")).Click();
                                                    }
                                                    messageAlert = await AlertIsPresent();
                                                    break;
                                                case "É necessário informar a INSCRIÇÃO IMOBILIÁRIA!":
                                                case "Inscrição Imobiliária Inválida!":

                                                    if (string.IsNullOrEmpty(messageAlert) && chrome.PageSource.Contains("Nesta página, você poderá imprimir a Certidão de Situação Fiscal e Enfitêutica"))
                                                    {
                                                        currentPage = true;
                                                    }
                                                    else
                                                    {
                                                        await WriteLog(messageAlert);
                                                        errors.Add(new GlobalError()
                                                        {
                                                            Code = 1,
                                                            Field = "IptuNumber",
                                                            Message = messageAlert
                                                        });

                                                        requisition.Errors = errors;
                                                        requisition.Status = Common.Enumerable.APIStatus.Error;
                                                        httpClientFWPut.Put<RequisitionModel, RequisitionModel>(new[] { "" }, requisition);

                                                        hasError = true;
                                                        currentPage = false;
                                                    }

                                                    break;

                                                default:

                                                    chrome.SwitchTo().Window(chrome.WindowHandles[1]);
                                                    System.Threading.Thread.Sleep(timeoutElement*2);
                                                    chrome.FindElement(By.TagName("html")).SendKeys(Keys.Return);

                                                    chrome.SwitchTo().Window(currentWindowsHandler);
                                                    System.Threading.Thread.Sleep(timeoutElement * 3);
                                                    chrome.FindElementByTagName("html").SendKeys(Keys.Command + Keys.Shift + "5");
                                                    System.Threading.Thread.Sleep(timeoutElement * 3);

                                                    var billetPrinted = Directory.GetFiles(dirDownload);
                                                    var billetPrintedBase64 = await FileToBase64(billetPrinted[0]);

                                                    requisition.Status = Common.Enumerable.APIStatus.Success;
                                                    requisition.UrlCertificate = "https://ifacilita.s3.us-east-2.amazonaws.com/" + new Common.Impl.S3().SaveFile(billetPrintedBase64, ".jpg");
                                                    httpClientFWPut.Put<RequisitionModel, RequisitionModel>(new[] { "" }, requisition);
                                                    
                                                    currentPage = false;
                                                    hasError = true;

                                                    billetPrinted.ToList().ForEach(bill => System.IO.File.Delete(bill));
                                                    Directory.Delete(dirDownload, true);

                                                    break;
                                            }

                                            if (currentPage) //reiniciar o processo
                                                break;

                                            if (hasError && !currentPage) break;
                                        }
                                    }

                                    await WriteLog($"Processo para a requisição {requisition.Id} finalizado com sucesso");
                                }
                            }
                        }
                        else
                        {
                            await WriteLog("Não foram encontradas requisições pendentes de processamento.");
                        }
                    }
                    catch (Exception ex)
                    {
                        await WriteLog(ex.Message);
                    }

                    await WriteLog("Aguardando próximo ciclo de processamento");
                    System.Threading.Thread.Sleep(30 * timeoutElement);
                }

            }
            catch (Exception ex)
            {
                await WriteLog(ex.Message);
            }
        }
    }
}
