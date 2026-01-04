using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.OnusReal.RPA
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            Console.Title = "[RPA] RealOnus -  Ônus Reais";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            await WriteLog("Chave de Inicialização: " + envStart);
            await WriteLog("Iniciando RPA OnusReal");
            await WriteLog("Ambiente de execução: " + environmentName);
            LocalLog.WriteLogStart(Configuration, "[RPA] RealOnus", "Ônus Reais");

            IS3 s3 = new Common.Impl.S3();
            IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW(Configuration["Api:Get"]);
            IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW(Configuration["Api:Put"]);

            while (true)
            {
                await WriteLog("Obtendo requisições pendentes");

                Common.HttpResult<Model.Requisition> get = httpClientFW.Get<Model.Requisition>(new[] { "" });

                if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                {
                    await WriteLog("Requisições pedentes foram encontradas. Iniciando o processamento");

                    Model.Requisition requisition = get.Value;
                    var options = new ChromeOptions();
                    ChromeDriverService service = null;
                    var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");

                    try
                    {
                        string downloadDirectory = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

                        var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
                        service = ChromeDriverService.CreateDefaultService(pathDrive);
                        service.HideCommandPromptWindow = true;

                        options.BinaryLocation = Configuration["ChromeBinary:Path"];

                        if (!Directory.Exists(dirDownload))
                            Directory.CreateDirectory(dirDownload);

                        options.AddUserProfilePreference("download.default_directory", dirDownload);
                        options.AddUserProfilePreference("disable-popup-blocking", "true");

                        var pathToExtension = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                        options.AddArgument("load-extension=" + pathToExtension);

                    }
                    catch (Exception ex)
                    {
                        await WriteLog("Houve uma falha na criação do driver do Chrome." + ex.Message);
                        System.Threading.Thread.Sleep(30 * 1000);
                        continue;
                    }

                    using ChromeDriver driver = new ChromeDriver(service, options);
                    const int timerInterval = 1000;
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    driver.Manage().Window.Maximize();

                    string valueRegistry = (requisition.NumCartorio < 10 ? "0" + requisition.NumCartorio.ToString() : requisition.NumCartorio.ToString()) + "º";

                    if (requisition.DocumentVisualization)
                    {
                        try
                        {
                            #region Task do Robo
                            var loginResult = await Login(driver);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.LinkText("Matrícula Online")).Location.Y});");
                            var btnMatricula = WebDriverExtensions.FindElement(driver, By.LinkText("Matrícula Online"));
                            btnMatricula.Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("menu_MatOnLine")).Location.Y});");
                            var btnMatOnline = WebDriverExtensions.FindElement(driver, By.Id("menu_MatOnLine"));
                            btnMatOnline.Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("imbStatusCertidao")).Location.Y});");
                            var btnImbStatusCertidao = WebDriverExtensions.FindElement(driver, By.Id("imbStatusCertidao"));
                            btnImbStatusCertidao.Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("chkModalAlerta")).Location.Y});");
                            var chkModalAlerta = WebDriverExtensions.FindElement(driver, By.Id("chkModalAlerta"));
                            chkModalAlerta.Click();

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnProsseguir")).Location.Y});");
                            var btnProsseguirCheck = driver.FindElement(By.Id("btnProsseguir"));
                            btnProsseguirCheck.Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            driver.Navigate().GoToUrl("https://www.registradores.org.br/VisualizarMatricula/frmContratoVM.aspx?comprarcredito=&UF=26");
                            System.Threading.Thread.Sleep(timerInterval);

                            while (true)
                            {
                                try
                                {
                                    js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("chkConcordar")).Location.Y});");
                                    var chkConcordar = WebDriverExtensions.FindElement(driver, By.Id("chkConcordar"));
                                    chkConcordar.Click();
                                    System.Threading.Thread.Sleep(timerInterval);
                                    break;

                                }
                                catch (Exception ex)
                                {
                                    System.Threading.Thread.Sleep(timerInterval);
                                    await WriteLog($"Ocorreu um erro ao localizar o botão\n, tentando novamente {ex.Message}");
                                }
                            }

                            while (true)
                            {
                                try
                                {
                                    js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnProsseguir")).Location.Y});");

                                    var btnProsseguirModal = WebDriverExtensions.FindElement(driver, By.Id("btnProsseguir"));
                                    btnProsseguirModal.Click();
                                    System.Threading.Thread.Sleep(timerInterval);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    System.Threading.Thread.Sleep(timerInterval);
                                    await WriteLog($"Ocorreu um erro ao localizar o botão\n, tentando novamente {ex.Message}");
                                }
                            }

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("dpdCidade")).Location.Y});");
                            var dpdCidade = WebDriverExtensions.FindElement(driver, By.Id("dpdCidade"));
                            dpdCidade.Click();
                            {
                                var dropdown = driver.FindElement(By.Id("dpdCidade"));
                                dropdown.FindElement(By.XPath("//option[. = 'São Paulo - Capital']")).Click();
                            }

                            System.Threading.Thread.Sleep(timerInterval);

                            var dpdCidadePasso2 = WebDriverExtensions.FindElement(driver, By.Id("dpdCidade"));
                            dpdCidadePasso2.Click();

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("dpdCartorio")).Location.Y});");
                            var dpdCartorio = WebDriverExtensions.FindElement(driver, By.Id("dpdCartorio"));
                            dpdCartorio.Click();

                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("dpdCartorio")).Location.Y});");
                            var dpdCartorioPasso2 = WebDriverExtensions.FindElement(driver, By.Id("dpdCartorio"));
                            dpdCartorioPasso2.Click();

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.XPath($"//option[. = '{valueRegistry}']")).Location.Y});");
                            var selectCartorio = WebDriverExtensions.FindElement(driver, By.XPath($"//option[. = '{valueRegistry}']"));
                            selectCartorio.Click();

                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("txtMatricula")).Location.Y});");
                            var txtMatricola = WebDriverExtensions.FindElement(driver, By.Id("txtMatricula"));
                            txtMatricola.Click();
                            txtMatricola.SendKeys(requisition.NumMatricola);
                            System.Threading.Thread.Sleep(timerInterval);

                            while (true)
                            {
                                try
                                {
                                    js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnGerar")).Location.Y});");
                                    var btnGerar = driver.FindElement(By.Id("btnGerar"));
                                    btnGerar.Click();
                                    System.Threading.Thread.Sleep(timerInterval);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    System.Threading.Thread.Sleep(timerInterval);
                                    await WriteLog($"Ocorreu um erro ao localizar o botão\n, tentando novamente {ex.Message}");
                                }
                            }

                            var validation = AlertIsPresent(driver);
                            if (!string.IsNullOrEmpty(validation))
                            {
                                requisition.Status = Common.Enumerable.APIStatus.Error;
                                requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                                requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "numMatricola", Message = validation } };
                                HttpResult<Model.Requisition> responsePut = httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                await WriteLog(validation + " -> Requisição Id: " + requisition.Id);
                                continue;
                            }

                            var countIntent = 0;
                            var solved = true;

                            try
                            {
                                while (driver.FindElement(By.TagName("body")).FindElement(By.ClassName("antigate_solver")).Text != "Solved")
                                {
                                    if (countIntent >= (60 * 5))
                                    {
                                        solved = false;
                                        break;
                                    }

                                    await WriteLog("Aguardando resolução do reCaptcha");
                                    countIntent++;
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                            catch (Exception ex)
                            {
                                await WriteLog("Não foi encontrado reCaptcha na página.");
                            }

                            if (!solved)
                            {
                                var msg = "Não foi possível resolver o captcha. O processo será reiniciado.";
                                await WriteLog(msg);
                                requisition.Status = Common.Enumerable.APIStatus.Pending;
                                requisition.StatusProcess = Model.Status.Waiting;

                                List<GlobalError> erros = new List<GlobalError>();
                                if (requisition.Errors != null || requisition.Errors.Any())
                                    erros = requisition.Errors.ToList();

                                erros.Add(new GlobalError() { Code = 0, Field = "reCAPTCHA", Message = msg });
                                requisition.Errors = erros;

                                HttpResult<Model.Requisition> responsePut = httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                                continue;
                            }


                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnConcluir")).Location.Y});");
                            driver.FindElement(By.Id("btnConcluir")).Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            //Validar Matrícula - A matrícula informada não foi encontrada para visualização.
                            validation = AlertIsPresent(driver);
                            if (!string.IsNullOrEmpty(validation))
                            {
                                requisition.Status = Common.Enumerable.APIStatus.Error;
                                requisition.StatusProcess = Model.Status.ErrorOnCallback;
                                requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 1, Field = "numMatricola", Message = validation } };
                                HttpResult<Model.Requisition> responsePut = httpClientFWPut.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                                continue;
                            }

                            var protocol = driver.FindElement(By.Id("lblProtocolo")).Text;
                            requisition.Protocolo = protocol;

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnPDF")).Location.Y});");
                            var btnPdf = driver.FindElement(By.Id("btnPDF"));

                            btnPdf.Click();
                            #endregion

                            System.Threading.Thread.Sleep(timerInterval);

                            while (driver.PageSource.Contains("Aguarde enquanto o sistema gera o PDF"))
                            {
                                await WriteLog("Aguardando geração do PDF");
                                System.Threading.Thread.Sleep(1000);
                            }

                            #region Localizando e salvando pdf no S3

                            DirectoryInfo directoryInfo = new DirectoryInfo(dirDownload);
                            string fileName = "";
                            foreach (FileInfo file in directoryInfo.GetFiles())
                            {
                                if (file.Extension.Contains(".pdf") && file.Name.Contains(requisition.NumMatricola))
                                {
                                    dirDownload = file.FullName;
                                    fileName = file.Name;
                                    break;
                                }
                            }

                            using (WebClient client = new WebClient())
                            {

                                string baseContract64 = FileToBase64(dirDownload).Result;
                                string keyS3 = s3.SaveFile(baseContract64, ".pdf");
                                requisition.s3patch = $"https://ifacilita.s3.us-east-2.amazonaws.com/{keyS3}";

                                File.Delete(dirDownload);
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            requisition.StatusProcess = Model.Status.ErrorOnProcessing;
                            requisition.StatusModified = DateTime.Now;

                            httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                            Console.WriteLine($"Ocorreu um erro em um dos processos do ChomeDriver {ex.Message}");
                        }
                    }
                    else
                    {
                        try
                        {
                            _ = await Login(driver);
                            driver.Navigate().GoToUrl(Configuration["Urls:Certificate"]);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.LinkText("Certidão Digital")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.LinkText("Certidão Digital")).Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.XPath("//a[text()='Novo Pedido']")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.XPath("//a[text()='Novo Pedido']")).Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            var windowCurrentHandler = driver.CurrentWindowHandle;
                            driver.SwitchTo().Frame(0);

                            driver.ExecuteScript("selectState(26,'São Paulo')");

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Contrato_ckbConcordar")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("Contrato_ckbConcordar")).Click();
                            System.Threading.Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Contrato_btnGoNext")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("Contrato_btnGoNext")).Click();
                            Thread.Sleep(timerInterval);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Cartorio_ddlCidade")).Location.Y});");
                            new SelectElement(driver.FindElement(By.Id("Cartorio_ddlCidade"))).SelectByValue("1");
                            Thread.Sleep(timerInterval * 3);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Cartorio_ddlCartorio")).Location.Y});");
                            new SelectElement(driver.FindElement(By.Id("Cartorio_ddlCartorio"))).SelectByValue(requisition.NumCartorio.ToString());
                            Thread.Sleep(timerInterval * 3);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Cartorio_btnGoNext")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("Cartorio_btnGoNext")).Click();
                            Thread.Sleep(timerInterval);

                            //Tipo Certidão *
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("TipoCertidao_ddlTipoCertidao")).Location.Y});");
                            new SelectElement(driver.FindElement(By.Id("TipoCertidao_ddlTipoCertidao"))).SelectByValue("3");
                            Thread.Sleep(timerInterval * 3);

                            //Pedido Por *
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("TipoCertidao_ddlPedidoPor")).Location.Y});");
                            new SelectElement(driver.FindElement(By.Id("TipoCertidao_ddlPedidoPor"))).SelectByValue("4");
                            Thread.Sleep(timerInterval * 3);

                            //Prosseguir
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("TipoCertidao_btnGoNext")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("TipoCertidao_btnGoNext")).Click();
                            Thread.Sleep(timerInterval);

                            //Informe a(s) matrícula(s) abaixo: * 
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("txtTag")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("txtTag")).SendKeys(requisition.NumMatricola);
                            Thread.Sleep(timerInterval);

                            //Prosseguir
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("PorMatriculaComComplemento_btnGoNext")).Location.Y});");
                            WebDriverExtensions.FindElement(driver, By.Id("PorMatriculaComComplemento_btnGoNext")).Click();
                            Thread.Sleep(timerInterval * 3);

                            //Concluir Pedido
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("Confirmacao_btnConcluirPedido")).Location.Y});");
                            //WebDriverExtensions.FindElement(driver, By.Id("Confirmacao_btnConcluirPedido")).Click();
                            Thread.Sleep(timerInterval * 3);

                            //Obter dados do Protocolo
                            //TODO - ATENÇÃO -> A recuperação do protocolo será feita com o primeiro pedido no site.

                            await WriteLog("Gerando documento temporário para a certidão");

                            string baseContract64 = FileToBase64("Documents/certidao_onus_reais.pdf").Result;
                            string keyS3 = s3.SaveFile(baseContract64, ".pdf");
                            requisition.s3patch = $"https://ifacilita.s3.us-east-2.amazonaws.com/{keyS3}";
                        }
                        catch (Exception ex)
                        {
                            await WriteLog("Ouve uma falha na tentatia de solicitação da Certidão: " + ex.Message);
                            await WriteLog("Uma nova tentativa será realizada para essa requisição: " + requisition.Id);
                        }
                    }

                    try
                    {
                        #region Operações de callback para Api RPA e Api Core

                        IHttpClientFW httpClientFW2 = new Common.Impl.HttpClientFW(Configuration["Api:Put"]);
                        HttpResult<Model.Requisition> postResult = httpClientFW2.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                        if (postResult.ActionResult is OkResult)
                        {
                            requisition.StatusModified = DateTime.Now;
                            requisition.Status = Common.Enumerable.APIStatus.Success;
                            requisition.StatusProcess = Model.Status.Finished;
                            requisition.Expiration = DateTime.Now.AddMonths(3).AddDays(-1);
                            requisition.Received = DateTime.Now;

                            if (!string.IsNullOrEmpty(requisition.UrlCallback))
                            {
                                var callbackResult = new Common.Impl.HttpClientFW(requisition.UrlCallback).Post<object, object>(new[] { "" }, new { requisition.Id, orderId = "", certiticateType = "RealOnus" });

                                if (!(callbackResult.ActionResult is OkResult))
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
                            var response = (BadRequestObjectResult)postResult.ActionResult;
                            var exceptionException = (Exception)response.Value;

                            requisition.StatusProcess = Model.Status.ErrorOnCallback;
                            requisition.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "Put", Message = exceptionException.Message } };
                        }

                        await WriteLog("Atualizando dados da requisição");
                        httpClientFW2.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                        await WriteLog("Processo finalizado com sucesso");

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        await WriteLog($"Não foi possível atualizar as informações da requisição: {requisition.Id}, a mensagem do servidor foi: {ex.Message}");
                        await WriteLog($"A requisição: {requisition.Id} será enviada para a fila e reprocessada");
                    }
                }
                else
                {
                    await WriteLog("Nenhuma requisição pendente para processamento");
                }

                await WriteLog("Aguardando próximo ciclo de processamento");
                System.Threading.Thread.Sleep(30 * 1000);
            }

        }

        public async static Task<bool> Login(ChromeDriver driver, int timerInterval = 1000)
        {
            try
            {
                string user = Configuration["user:email"];
                string password = Configuration["user:password"];

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                driver.Navigate().GoToUrl(Configuration["Urls:Base"]);

                js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("txtEmail")).Location.Y});");
                driver.FindElement(By.Id("txtEmail")).SendKeys(user);

                js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("txtSenha")).Location.Y});");
                driver.FindElement(By.Id("txtSenha")).SendKeys(password);

                while (true)
                {
                    try
                    {
                        js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btnProsseguir")).Location.Y});");
                        var btnProsseguir = driver.FindElement(By.Id("btnProsseguir"));
                        btnProsseguir.Click();
                        System.Threading.Thread.Sleep(timerInterval * 2);
                        break;
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(timerInterval * 2);
                        await WriteLog($"Ocorreu um erro ao localizar o botão\n, tentando novamente {ex.Message}");
                    }
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        private async static Task<bool> WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/OnusReal-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";
                await File.AppendAllLinesAsync(pathLog, new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }

            return true;
        }

        private async static Task<bool> WriteLogStart(string key)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/ifacilitaStart-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{key}] - OnusReal ->  Ônus Reais";
                await File.AppendAllLinesAsync(pathLog, new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }

            return true;
        }

        private static async Task<string> FileToBase64(string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
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
