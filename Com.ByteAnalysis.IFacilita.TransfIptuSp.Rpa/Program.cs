using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Com.ByteAnalysis.IFacilita.TransfIptuSp.Model;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Rpa
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

                var pathLog = $"{Configuration["OutputLogs:Path"]}/IptuDebit-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";
                File.AppendAllLines(pathLog, new string[] { msg });
                Console.Out.WriteLine(msg);
            }
            catch { }
        }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] Transferencia IPTU - Transferencia de titularidade de IPTU";

            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] Transferencia IPTU", "Transferencia de titularidade de IPTU");

            WriteLog("Iniciando RPA Débitos do IPTU");
            WriteLog("Ambiente de execução: " + environmentName);

            string urlApiBase = Configuration["Api:UrlBase"];

            Common.IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW($"{urlApiBase}/{Configuration["Api:GetPending"]}");
            Common.IHttpClientFW httpClientFWPut = new Common.Impl.HttpClientFW($"{urlApiBase}/{Configuration["Api:Put"]}");
            IS3 s3 = new Common.Impl.S3();

            int timeoutCicle = 1000 * 30;
            int timeoutWait = 1000;

            while (true)
            {
                try
                {
                    WriteLog("Procurando requisições pendentes");
                    Common.HttpResult<IEnumerable<RequisitionModel>> get = httpClientFW.Get<IEnumerable<RequisitionModel>>(null);

                    if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                    {
                        WriteLog("Requisição encontrada");
                        WriteLog("Configurando driver do Chrome");

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

                        RequisitionModel requisition = get.Value.FirstOrDefault();

                        using (ChromeDriver chrome = new ChromeDriver(service, options))
                        {
                            chrome.Manage().Window.Maximize();
                            chrome.Navigate().GoToUrl(Configuration["Urls:Login"]);

                            chrome.FindElement(By.Id("formBody_formBody_txtUser")).SendKeys(Configuration["Urls:User"]);
                            Thread.Sleep(timeoutWait);

                            chrome.FindElement(By.Id("formBody_formBody_txtPassword")).SendKeys(Configuration["Urls:Password"]);
                            Thread.Sleep(timeoutWait);

                            string imagem = chrome.FindElement(By.Id("imgCaptcha")).GetAttribute("src");
                            Screenshot image = ((ITakesScreenshot)chrome.FindElement(By.Id("imgCaptcha"))).GetScreenshot();
                            string imgValue = CaptchaSolveWithBase64(image.AsBase64EncodedString).Result;
                            chrome.FindElement(By.Id("txtValidacao")).SendKeys(imgValue);
                            Thread.Sleep(timeoutWait);

                            chrome.FindElement(By.Id("formBody_formBody_btnLogin")).Click();
                            Thread.Sleep(timeoutWait);

                            chrome.Navigate().GoToUrl(Configuration["Urls:IptuSearch"]);
                            Thread.Sleep(timeoutWait);

                            //Setor
                            chrome.FindElement(By.Id("txtSetor")).SendKeys(requisition.IptuSql.Substring(0, 3));
                            Thread.Sleep(timeoutWait / 2);

                            //Quadra
                            chrome.FindElement(By.Id("txtQuadra")).SendKeys(requisition.IptuSql.Substring(3, 3));
                            Thread.Sleep(timeoutWait / 2);

                            //Lote
                            chrome.FindElement(By.Id("txtLote")).SendKeys(requisition.IptuSql.Substring(6, 4));
                            Thread.Sleep(timeoutWait / 2);

                            //Digito
                            chrome.FindElement(By.Id("txtDigito")).SendKeys(requisition.IptuSql.Substring(10, 1));
                            Thread.Sleep(timeoutWait / 2);

                            chrome.FindElement(By.Id("_BtnAvancarDasii")).Click();
                            Thread.Sleep(timeoutWait / 2);

                            //Atualização feita pelo COMPRADOR
                            if (requisition.BuyerUpdate)
                            {
                                chrome.FindElement(By.Id("rdbAtualizacaoComprador")).Click();
                                Thread.Sleep(timeoutWait);
                            }
                            else //Atualização feita pelo VENDEDOR
                            {
                                chrome.FindElement(By.Id("rdbAtualizacaoVendedor")).Click();
                                Thread.Sleep(timeoutWait);
                            }

                            //Nome do contribuinte
                            chrome.FindElement(By.Id("txtNome")).SendKeys(requisition.Name);
                            Thread.Sleep(timeoutWait);

                            //CPF
                            if (requisition.PhysicalPerson)
                            {
                                chrome.FindElement(By.Id("rb1")).Click();
                                Thread.Sleep(timeoutWait);
                            }
                            else //CNPJ
                            {
                                chrome.FindElement(By.Id("rb2")).Click();
                                Thread.Sleep(timeoutWait);
                            }

                            //CPF/CNPJ:
                            chrome.FindElement(By.Id("txtCpf")).SendKeys(requisition.Document);
                            Thread.Sleep(timeoutWait);

                            //Tipo de documento de propriedade
                            var selectRegistry = new OpenQA.Selenium.Support.UI.SelectElement(chrome.FindElement(By.Id("cboQualificacao")));
                            selectRegistry.SelectByValue(requisition.DocumentType.ToString());
                            Thread.Sleep(timeoutWait);

                            //Número da Matrícula:
                            chrome.FindElement(By.Id("txtNumeroMatricula")).SendKeys(requisition.Registration);
                            Thread.Sleep(timeoutWait);

                            //Número do Cartório:
                            chrome.FindElement(By.Id("txtNumeroCartorio")).SendKeys(requisition.Registry);
                            Thread.Sleep(timeoutWait);

                            //Número do Cartório:
                            chrome.FindElement(By.Id("txtDataAquisicaoImovel")).SendKeys(requisition.DateAcquisition.ToString("dd/MM/yyyy"));
                            Thread.Sleep(timeoutWait);

                            //DATA DE PAGAMENTO DO IPTU
                            var selectDateDue = new OpenQA.Selenium.Support.UI.SelectElement(chrome.FindElement(By.Id("cboPagto")));
                            selectDateDue.SelectByValue(requisition.PayDay.ToString("00"));
                            Thread.Sleep(timeoutWait);

                            System.Text.StringBuilder currentHandler = new System.Text.StringBuilder();
                            currentHandler.Append(chrome.CurrentWindowHandle);

                            if (!requisition.AddressDeliveryIptuEquals)
                            {
                                chrome.FindElement(By.Id("rbEndImovelNao")).Click();
                                Thread.Sleep(timeoutWait);

                                foreach (var wind in chrome.WindowHandles)
                                {
                                    chrome.SwitchTo().Window(wind);
                                    var windowTitle = chrome.Title;

                                    if (!windowTitle.Contains("Endereço do Proprietário"))
                                        continue;

                                    chrome.FindElement(By.Id("txtCepCont")).SendKeys(requisition.Cep);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtEnderecoCont")).SendKeys(requisition.Address);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtNumeroCont")).SendKeys(requisition.Number);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtComplCont")).SendKeys(requisition.Complement);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtBairroCont")).SendKeys(requisition.Neighborhood);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtCidadeCont")).SendKeys(requisition.City);
                                    Thread.Sleep(timeoutWait);

                                    var selectUf = new OpenQA.Selenium.Support.UI.SelectElement(chrome.FindElement(By.Id("cboUfCont")));
                                    selectUf.SelectByValue(requisition.Uf);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtEmail")).SendKeys(requisition.Email);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtDDDTelefone")).SendKeys(requisition.Ddd);
                                    Thread.Sleep(timeoutWait);

                                    chrome.FindElement(By.Id("txtTelefone")).SendKeys(requisition.Phone);
                                    Thread.Sleep(timeoutWait);

                                    if (requisition.AddressDeliveryIptuDeclarant)
                                    {
                                        chrome.FindElement(By.Id("rbDeclarante")).Click();
                                        Thread.Sleep(timeoutWait);
                                    }
                                    else
                                    {
                                        chrome.FindElement(By.Id("rbImovel")).Click();
                                        Thread.Sleep(timeoutWait);
                                    }

                                    chrome.FindElement(By.Id("btnOk")).Click();
                                    Thread.Sleep(timeoutWait);

                                }
                            }

                            chrome.SwitchTo().Window(currentHandler.ToString());

                            //OUTROS CONTRIBUINTES 
                            if (requisition.OtherOwners != null && requisition.OtherOwners.Count() > 0)
                            {
                                currentHandler.Append(chrome.CurrentWindowHandle);
                                chrome.FindElement(By.Id("rbOutrosSim")).Click();
                                Thread.Sleep(timeoutWait);

                                foreach (var wind in chrome.WindowHandles)
                                {
                                    chrome.SwitchTo().Window(wind);
                                    var windowTitle = chrome.Title;

                                    if (!windowTitle.Contains("InclusaoProprietarios"))
                                        continue;

                                    foreach (var other in requisition.OtherOwners)
                                    {
                                        chrome.FindElement(By.Id("rbOutrosSim")).Click();
                                        Thread.Sleep(timeoutWait);

                                        chrome.FindElement(By.Id("txtNomeOutros")).SendKeys(other.Name);
                                        Thread.Sleep(timeoutWait);

                                        chrome.FindElement(By.Id("txtCpfOutros")).SendKeys(other.Document);
                                        Thread.Sleep(timeoutWait);

                                        selectRegistry = new OpenQA.Selenium.Support.UI.SelectElement(chrome.FindElement(By.Id("cboQualificOutros")));
                                        selectRegistry.SelectByValue(requisition.DocumentType.ToString());
                                        Thread.Sleep(timeoutWait);

                                        chrome.FindElement(By.Id("btnOk")).Click();
                                        Thread.Sleep(timeoutWait);
                                    }
                                }
                            }

                            chrome.SwitchTo().Window(currentHandler.ToString());
                            Thread.Sleep(timeoutWait);

                            //Encaminhar
                            chrome.FindElement(By.Id("btnEncaminhar")).Click();
                            Thread.Sleep(timeoutWait);

                            Screenshot imageRequest = ((ITakesScreenshot)chrome.FindElement(By.Id("frmCadastro"))).GetScreenshot();
                            requisition.UrlRequisition = $"https://ifacilita.s3.us-east-2.amazonaws.com/{s3.SaveFile(imageRequest.AsBase64EncodedString,".jpg")}"; 

                            requisition.Status = Common.Enumerable.APIStatus.Success;
                            requisition.Pending = false;

                            var responsePut = httpClientFWPut.Put<RequisitionModel, RequisitionModel>(new[] { "" }, requisition);
                        }
                    }
                    else
                    {
                        WriteLog("Nenhuma requisição pendente para processamento");
                    }

                    WriteLog("Aguardando próximo ciclo de processamento");
                    Thread.Sleep(timeoutCicle);
                }
                catch (Exception ex)
                {
                    WriteLog("Houve uma falha no processamento da requisição: " + ex.Message);
                }
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

    }
}
