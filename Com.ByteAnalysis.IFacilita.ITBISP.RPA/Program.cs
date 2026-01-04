
using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.EmailService;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.ITBISP.RPA
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        private static void WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/ITBISP-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch { }
        }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] ITBI.SP - Impostos de Transmissão de Bens Imóveis";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            WriteLog("Chave de Inicialização: " + envStart);
            WriteLog("Iniciando ITBI.SP - Impostos de Transmissão de Bens Imóveis");
            WriteLog("Ambiente de execução: " + environmentName);
            LocalLog.WriteLogStart(Configuration, "[RPA] ITBI.SP", "Impostos de Transmissão de Bens Imóveis");

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
                    Common.HttpResult<Model.RequisitionItbiModel> get = httpClientFW.Get<Model.RequisitionItbiModel>(new[] { "" });

                    if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                    {
                        WriteLog("Existem requisições pendentes de processamento.");

                        Model.RequisitionItbiModel requisition = get.Value;
                        WriteLog("Iniciando processamento da requisição: " + requisition.Id);

                        ChromeDriverService service = null;
                        ChromeOptions options = new ChromeOptions();
                        var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");

                        try
                        {
                            if (!Directory.Exists(dirDownload))
                                Directory.CreateDirectory(dirDownload);

                            var pathPluginCaptcha = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                            var pathPluginPrint = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "scrnli");

                            WriteLog("Configurando plugin anti-captcha. " + pathPluginCaptcha);

                            options.AddUserProfilePreference("download.default_directory", dirDownload);
                            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                            options.AddUserProfilePreference("download.prompt_for_download", false);
                            options.AddUserProfilePreference("download.directory_upgrade", true);
                            options.AddUserProfilePreference("safebrowsing_for_trusted_sources_enabled", false);
                            options.AddUserProfilePreference("safebrowsing.enabled", false);

                            options.AddArguments($"load-extension={pathPluginCaptcha}");
                            options.AddArguments($"load-extension={pathPluginPrint}");

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
                            continue;
                        }

                        using (ChromeDriver chrome = new ChromeDriver(service, options))
                        {
                            System.Threading.Thread.Sleep(timeoutElement * 3);
                            //Fechar tab de instalação do plugin
                            chrome.SwitchTo().Window(chrome.WindowHandles[1]).Close();
                            chrome.SwitchTo().Window(chrome.WindowHandles[0]);

                            chrome.Manage().Window.Maximize();
                            chrome.Navigate().GoToUrl(Configuration["Urls:Itbi"]);

                            //Natureza da Transação
                            var selectHtml = new SelectElement(chrome.FindElement(By.Id("ddlNatureza")));
                            selectHtml.SelectByValue(requisition.Transaction);
                            System.Threading.Thread.Sleep(timeoutElement);

                            //SQL
                            chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[1]/div/div[2]/input")).SendKeys(requisition.Iptu);
                            System.Threading.Thread.Sleep(timeoutElement * 3);

                            //Comprador
                            for (int i = 1; i <= requisition.Buyers.Count(); i++)
                            {
                                //Documento
                                chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[2]/div[{i}]/div[1]/input")).SendKeys(Convert.ToInt64(requisition.Buyers.ToList()[i - 1].Document).ToString(@"000\.000\.000\-00"));
                                System.Threading.Thread.Sleep(timeoutElement);

                                //Nome
                                chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[2]/div[{i}]/div[2]/div/div/input")).Click();
                                //chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[2]/div[{i}]/div[2]/div/div/input")).SendKeys(requisition.Buyers.ToList()[i-1].Name);
                                System.Threading.Thread.Sleep(timeoutElement);

                                if (i < requisition.Buyers.Count())
                                    chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[2]/div[3]/button")).Click();
                            }

                            //vendedor
                            for (int i = 1; i <= requisition.Sellers.Count(); i++)
                            {
                                //Documento
                                chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[3]/div[{i}]/div[1]/input")).SendKeys(Convert.ToInt64(requisition.Sellers.ToList()[i - 1].Document).ToString(@"000\.000\.000\-00"));
                                System.Threading.Thread.Sleep(timeoutElement);

                                //Nome
                                chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[3]/div[{i}]/div[2]/div/div/input")).Click();
                                //chrome.FindElement(By.XPath($"/html/body/div[1]/main/form/div[3]/div/fieldset[3]/div[{i}]/div[2]/div/div/input")).SendKeys(requisition.Sellers.ToList()[i-1].Name);
                                System.Threading.Thread.Sleep(timeoutElement);

                                if (i < requisition.Buyers.Count())
                                    chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[3]/div[3]/button")).Click();
                            }

                            //Valor
                            chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[1]/input")).SendKeys(string.Format("{0:C}", requisition.Value).Replace("R$", ""));
                            System.Threading.Thread.Sleep(timeoutElement);

                            //Tipo Financiamento
                            if (!string.IsNullOrEmpty(requisition.FinancingType) && requisition.FinancingType != "0")
                            {
                                var selectFinancing = new SelectElement(chrome.FindElement(By.Id("cboTpFinan")));
                                selectFinancing.SelectByValue(requisition.FinancingType);
                                System.Threading.Thread.Sleep(timeoutElement);

                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[2]/div[2]/input")).SendKeys(requisition.ValueFinancing.ToString());
                                System.Threading.Thread.Sleep(timeoutElement);
                            }

                            //ESTA SENDO TRANSMITIDA A TOTALIDADE DO IMÓVEL?
                            if (requisition.Totality)
                            {
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[3]/span[1]/label")).Click();
                                System.Threading.Thread.Sleep(timeoutElement);
                            }
                            else
                            {
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[3]/span[2]/label")).Click();
                                System.Threading.Thread.Sleep(timeoutElement);

                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[5]/input")).SendKeys(requisition.Proportion.ToString());
                                System.Threading.Thread.Sleep(timeoutElement);
                            }

                            //TIPO DE INSTRUMENTO
                            if (requisition.PublicScripture)
                            {
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[4]/div/span[2]/label")).Click();
                                System.Threading.Thread.Sleep(timeoutElement);

                                //CARTÓRIO DE NOTAS 
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[5]/input")).SendKeys(requisition.NotesOffice.ToString());
                                System.Threading.Thread.Sleep(timeoutElement);

                                //UF
                                var selectUf = new SelectElement(chrome.FindElement(By.Id("TxtUfCartorioNotas")));
                                selectUf.SelectByValue(requisition.Uf);
                                System.Threading.Thread.Sleep(timeoutElement);

                                //Municipio
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[6]/div[4]/input")).SendKeys(requisition.City);
                                System.Threading.Thread.Sleep(timeoutElement);

                                //DATA DO INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIRA *
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[6]/div/span/input")).SendKeys(requisition.DateEvent.ToString("dd/MM/yyyy"));
                                System.Threading.Thread.Sleep(timeoutElement);

                                //MATRÍCULA/TRANSCRIÇÃO DO CARTÓRIO DE REGISTRO DE IMÓVEL
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[8]/input")).SendKeys(requisition.Registration);
                                System.Threading.Thread.Sleep(timeoutElement);
                            }
                            else
                            {
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[4]/div/span[1]/label")).Click();
                                System.Threading.Thread.Sleep(timeoutElement);

                                //DATA DO INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIRA *
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[5]/div/span/input")).SendKeys(requisition.DateEvent.ToString("dd/MM/yyyy"));
                                System.Threading.Thread.Sleep(timeoutElement);

                                //MATRÍCULA/TRANSCRIÇÃO DO CARTÓRIO DE REGISTRO DE IMÓVEL
                                chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset[4]/div/div[7]/input")).SendKeys(requisition.Registration);
                                System.Threading.Thread.Sleep(timeoutElement);
                            }

                            //CARTÓRIO DE REGISTRO
                            var selectRegistry = new SelectElement(chrome.FindElement(By.Id("DdlCartorioRegistroImovel")));
                            selectRegistry.SelectByValue(requisition.Registry.ToString());
                            System.Threading.Thread.Sleep(timeoutElement);

                            //Avançar
                            chrome.FindElement(By.XPath("//button[contains(text(),'Avançar')]")).Click();
                            System.Threading.Thread.Sleep(timeoutElement);


                            var hasError = false;
                            var validations = chrome.FindElements(By.ClassName("help-block"));

                            foreach (var validation in validations)
                            {
                                if (validation.GetAttribute("style") == "")
                                {
                                    var parent = validation.FindElement(By.XPath("./../.."));
                                    var grandParent = parent.GetAttribute("class");

                                    var msgError = validation.Text;
                                    hasError = true;

                                    switch (grandParent)
                                    {
                                        case "dados-imovel":
                                            errors.Add(new GlobalError() { Code = 1, Field = "iptu", Message = $"{grandParent} - {msgError}" });
                                            break;
                                        case "dados-comprador":
                                            errors.Add(new GlobalError() { Code = 2, Field = "buyers", Message = $"{grandParent} - {msgError}" });
                                            break;
                                        case "dados-vendedor":
                                            errors.Add(new GlobalError() { Code = 3, Field = "sellers", Message = $"{grandParent} - {msgError}" });
                                            break;
                                        default:
                                            break;
                                    }

                                }
                            }

                            if (hasError)
                            {
                                requisition.Errors = errors;
                                requisition.Pending = false;
                                requisition.Status = Common.Enumerable.APIStatus.Error;

                                _ = httpClientFWPut.Put<RequisitionItbiModel, RequisitionItbiModel>(new[] { "" }, requisition);
                                continue;

                            }

                            //Calcular Impostos
                            chrome.FindElement(By.XPath("//button[contains(text(),'Calcular Imposto')]")).Click();
                            System.Threading.Thread.Sleep(timeoutElement);

                            var resultDate = chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[4]/input")).GetAttribute("value");
                            var resultValue = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[4]/div/div")).Text.Replace("R$", "").Trim().Replace(".", ""), cultureInfo);

                            requisition.Calculation = new Model.CalculationModel()
                            {
                                CalculationBase = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[2]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                Correction = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[6]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                Fine = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[3]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                Interest = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[7]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                Tax = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[5]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                Total = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[4]/div/div")).Text.Replace("R$", "").Trim().Replace(".", ""), cultureInfo),
                                ValueRef = Convert.ToDecimal(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[1]/div")).Text.Replace("R$", "").Trim().Replace(".", "")),
                                DateDue = Convert.ToDateTime(chrome.FindElement(By.XPath("/html/body/div[1]/main/form/div[3]/div/fieldset/div[3]/div[4]/input")).GetAttribute("value"), cultureInfo)

                            };

                            requisition.Pending = false;
                            requisition.Status = Common.Enumerable.APIStatus.Success;

                            if (!requisition.Approved)
                            {
                                _ = httpClientFWPut.Put<RequisitionItbiModel, RequisitionItbiModel>(new[] { "" }, requisition);
                            }
                            else
                            {
                                chrome.FindElement(By.XPath("//button[contains(text(),'Emitir Guia Pagamento')]")).Click();
                                System.Threading.Thread.Sleep(timeoutElement);

                                if (!Directory.Exists("download-billet"))
                                    Directory.CreateDirectory("download-billet");

                                //chrome.Manage().Window.FullScreen();

                                IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
                                var head = chrome.FindElement(By.TagName("header"));
                                js.ExecuteScript("arguments[0].remove();", head);
                                
                                var nav = chrome.FindElement(By.TagName("nav"));
                                js.ExecuteScript("arguments[0].remove();", nav);

                                var footer = chrome.FindElement(By.TagName("footer"));
                                js.ExecuteScript("arguments[0].remove();", footer);

                                //js.ExecuteScript("document.getElementById('supertop').setAttribute('style','display:none')");
                                //System.Threading.Thread.Sleep(timeoutElement / 3);

//                                js.ExecuteScript($"document.getElementsByTagName('footer')[0].style='display:none;'");
                                //System.Threading.Thread.Sleep(timeoutElement / 3);

                                js.ExecuteScript($"document.getElementsByClassName('botoes')[0].style='display:none;'");
                                System.Threading.Thread.Sleep(timeoutElement / 3);

                                //js.ExecuteScript($"document.getElementsByClassName('caixa-header')[0].style='display:none;'");
                                //System.Threading.Thread.Sleep(timeoutElement / 3);

                                //js.ExecuteScript($"document.getElementsByClassName('menu-transacao')[0].style='display:none;'");
                                //System.Threading.Thread.Sleep(timeoutElement / 3);

                                js.ExecuteScript($"document.getElementsByClassName('titulo-declaracao')[0].style='display:none;'");
                                System.Threading.Thread.Sleep(timeoutElement / 3);

                                js.ExecuteScript($"document.getElementsByClassName('subtitulo-declaracao')[0].style='display:none;'");
                                System.Threading.Thread.Sleep(timeoutElement / 3);

                                js.ExecuteScript($"document.getElementsByClassName('step-progressbar')[0].style='display:none;'");
                                System.Threading.Thread.Sleep(timeoutElement / 3);

                                js.ExecuteScript($"document.getElementsByClassName('divInstrucoes')[0].style='display:none;'");
                                System.Threading.Thread.Sleep(timeoutElement / 3);

                                var app = chrome.FindElement(By.Id("app"));
                                //js.ExecuteScript("arguments[0].classList.remove('conteudo');", app);

                                js.ExecuteScript("arguments[0].setAttribute('style','width:900px');", app);

                                //Printar Boleto
                                chrome.FindElementByTagName("html").SendKeys(Keys.Command + Keys.Shift + "5");
                                System.Threading.Thread.Sleep(timeoutElement * 3);

                                var billetPrinted = Directory.GetFiles(dirDownload);
                                var billetPrintedBase64 = FileToBase64(billetPrinted[0]);

                                requisition.UrlBillet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + new Common.Impl.S3().SaveFile(billetPrintedBase64, ".jpg");
                                _ = httpClientFWPut.Put<RequisitionItbiModel, RequisitionItbiModel>(new[] { "" }, requisition);

                                billetPrinted.ToList().ForEach(bill => System.IO.File.Delete(bill));
                                Directory.Delete(dirDownload,true);
                            }

                        }

                        WriteLog($"Processamento da requisição:{requisition.Id} finalizado com sucesso.");
                    }
                    else
                    {
                        WriteLog("Não existem requisições pendentes para processamento.");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("Houve uma falha no processamento da requisição. " + ex.Message);
                }
                finally
                {
                    WriteLog("Aguardando próximo ciclo de processamento");
                    System.Threading.Thread.Sleep(30 * 1000);
                }
            }
        }

        private static string FileToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
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

