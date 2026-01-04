using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.RGI.Robot
{
    public class Program
    {

        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "[RPA] RGI - Registro de imóveis";

            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.MongoDB(Configuration.GetValue<string>("DatabaseSettings:BaseLog"), collectionName: "RGI.RPA")
            .CreateLogger();

            var applicationSettings = Configuration.GetSection("ApplicationSettings").Get<Model.ApplicationSettings>();

            Common.IHttpClientFW httpClientFW = new Common.Impl.HttpClientFW("http://40.124.76.25:5900/api/Rgi");

            while (true)
            {
                Log.Information("calling HttpGet(to-proccess)");

                Common.HttpResult<Model.Requisition> get = httpClientFW.Get<Model.Requisition>(new[] { "to-proccess" });
                //Model.Requisition requisition = get.
                if (get.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                {
                    Common.IS3 s3 = new Common.Impl.S3();

                    Model.Requisition requisition = get.Value;

                    Log.Information("Proccessing Model.Requisition: " + Newtonsoft.Json.JsonConvert.SerializeObject(requisition));

                    var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
                    var service = ChromeDriverService.CreateDefaultService(pathDrive);
                    service.HideCommandPromptWindow = true;

                    var options = new ChromeOptions();
                    options.BinaryLocation = Program.Configuration["ChromeBinary:Path"];

                    var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
                    if (!Directory.Exists(dirDownload))
                        Directory.CreateDirectory(dirDownload);

                    options.AddUserProfilePreference("download.default_directory", dirDownload);
                    options.AddUserProfilePreference("disable-popup-blocking", "true");
                    options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

                    var pathToExtension = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
                    options.AddArgument("load-extension=" + pathToExtension);

                    Log.Information("Opening Chrome");
                    using (ChromeDriver driver = new ChromeDriver(service, options))
                    {
                        Log.Information("Navigate().GoToUrl(\"https://www.registradores.org.br/eProtocolo/DefaultAC.aspx\")");
                        
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl("https://www.registradores.org.br/eProtocolo/DefaultAC.aspx");

                        Log.Information("Login");
                        driver.FindElementById("txtEmail").SendKeys(applicationSettings.EProtocoloUsername);
                        driver.FindElementById("txtSenha").SendKeys(applicationSettings.EProtocoloPassword);
                        driver.FindElementById("btnProsseguir").SendKeys("\n");

                        Log.Information("successfully logged in");

                        Log.Information("Navigate().GoToUrl(\"https://www.registradores.org.br/eProtocolo/frm_cadastro_contrato.aspx?UF=19&convenio=1&xml=0&idc=&r=&agb=&req=0\")");
                        driver.Navigate().GoToUrl("https://www.registradores.org.br/eProtocolo/frm_cadastro_contrato.aspx?UF=19&convenio=1&xml=0&idc=&r=&agb=&req=0");
                        {
                            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElementById("dpdTp_Doc"));
                            selectElement.SelectByValue("1");
                        }

                        driver.FindElementById("txtDataTitulo").SendKeys(requisition.ShipmentData.TitleDate.ToString("ddMMyyyy"));
                        driver.FindElementById("txtLivro").SendKeys(requisition.ShipmentData.Book.ToString());
                        driver.FindElementById("txtFolha").SendKeys(requisition.ShipmentData.Sheet.ToString());

                        System.Threading.Thread.Sleep(3000);
                        Log.Information("Solving Captcha...");
                        for (int i = 0; i < 1000; i++)
                        {
                            string text = driver.FindElementByTagName("body").FindElement(OpenQA.Selenium.By.ClassName("antigate_solver")).Text;
                            if (text == "Solved")
                            {
                                break;
                            }
                            Log.Information($"Waiting {i} of 1000");
                            System.Threading.Thread.Sleep(5000);
                        }

                        Log.Information("Solved!!!");
                        Log.Information("Informing the Sellers");

                        foreach (var seller in requisition.ShipmentData.Sellers)
                        {
                            string screenshot = null;
                            try
                            {
                                {
                                    var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElementById("divTpDocOutorgante").FindElement(OpenQA.Selenium.By.TagName("select")));
                                    if (seller.TypeOfDocument == Model.TypeOfDocument.CPF)
                                        selectElement.SelectByValue("cpf");
                                    else
                                        selectElement.SelectByValue("cnpj");
                                }
                                driver.FindElementById("txtCPFOutorgante").SendKeys(seller.DocumentNumber.ToString().PadLeft(11, '0'));
                                driver.FindElementById("btnPesquisarOutorgante").SendKeys(" ");



                                screenshot = driver.GetScreenshot().AsBase64EncodedString;

                                for (int i = 0; i < 100; i++)
                                {
                                    var element = driver.FindElementById("txtNomeOutorgante").GetAttribute("value");
                                    if (element != "" && !element.Contains("AGUARDE"))
                                        break;
                                    System.Threading.Thread.Sleep(100);
                                }
                                System.Threading.Thread.Sleep(5000);
                                driver.FindElementByClassName("outorgante-outorgado").FindElement(OpenQA.Selenium.By.ClassName("adicionar")).Click();
                            }
                            catch (OpenQA.Selenium.UnhandledAlertException err)
                            {
                                screenshot = s3.SaveFile(screenshot, ".png");

                                Log.Error(err, err.Message);
                                if (requisition.Errors == null)
                                    requisition.Errors = new List<Common.Exceptions.GlobalError>();
                                requisition.Status = Common.Enumerable.APIStatus.Error;
                                requisition.Errors = requisition.Errors.Append<Common.Exceptions.GlobalError>(new Common.Exceptions.GlobalError()
                                {
                                    Code = 0,
                                    Message = "CPF do Vendedor Invalido!",
                                    PathImageError = screenshot,
                                    Field = "CPF"
                                });
                                httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                //Common.IS3 s3 = new Common.Impl.S3();

                                //requisition.EPropocolo.ConfirmationScreen = s3.SaveFile(chrome.GetScreenshot().AsBase64EncodedString, ".png");
                                break;
                            }
                        }

                        Log.Information("Informing the Buyers");
                        foreach (var buyer in requisition.ShipmentData.Buyers)
                        {
                            string screenshot = null;
                            try
                            {
                                {
                                    var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElementById("divTpDocOutorgado").FindElement(OpenQA.Selenium.By.TagName("select")));
                                    if (buyer.TypeOfDocument == Model.TypeOfDocument.CPF)
                                        selectElement.SelectByValue("cpf");
                                    else
                                        selectElement.SelectByValue("cnpj");
                                }
                                driver.FindElementById("txtCPFOutorgado").SendKeys(buyer.DocumentNumber.ToString().PadLeft(11, '0'));
                                driver.FindElementById("btnPesquisarOutorgado").SendKeys(" ");



                                for (int i = 0; i < 100; i++)
                                {
                                    var element = driver.FindElementById("txtNomeOutorgado").GetAttribute("value");
                                    if (element != "" && !element.Contains("AGUARDE"))
                                        break;
                                    System.Threading.Thread.Sleep(100);
                                }
                                System.Threading.Thread.Sleep(5000);
                                driver.FindElementByClassName("outorgado-holder").FindElement(OpenQA.Selenium.By.ClassName("adicionar")).Click();
                            }
                            catch (OpenQA.Selenium.UnhandledAlertException err)
                            {
                                screenshot = s3.SaveFile(screenshot, ".png");

                                Log.Error(err, err.Message);
                                if (requisition.Errors == null)
                                    requisition.Errors = new List<Common.Exceptions.GlobalError>();
                                requisition.Status = Common.Enumerable.APIStatus.Error;
                                requisition.Errors = requisition.Errors.Append<Common.Exceptions.GlobalError>(new Common.Exceptions.GlobalError()
                                {
                                    Code = 1,
                                    Message = "CPF do Comprador Invalido!",
                                    PathImageError = screenshot,
                                    Field = "CPF"
                                });
                                httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);
                                break;
                                //Common.IS3 s3 = new Common.Impl.S3();

                                //requisition.EPropocolo.ConfirmationScreen = s3.SaveFile(chrome.GetScreenshot().AsBase64EncodedString, ".png");

                            }
                        }
                        if (requisition.Errors != null)
                            continue;

                        Log.Information("Seleting City and Registry");


                        {
                            System.Threading.Thread.Sleep(2000);
                            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElementById("dpdcomarca"));
                            System.Threading.Thread.Sleep(2000);
                            selectElement.SelectByValue(Convert.ToString((int)requisition.ShipmentData.ReceivingNotary.City));
                        }
                        {
                            System.Threading.Thread.Sleep(2000);
                            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElementById("dpd_cartorios"));


                            System.Threading.Thread.Sleep(5000);
                            selectElement.SelectByText(requisition.ShipmentData.ReceivingNotary.NotaryNumber.ToString().PadLeft(2, '0') + "º");
                        }
                        Log.Information("Seleting DDD, Phone");

                        driver.FindElementById("txt_ddd").SendKeys(applicationSettings.PresenterDDD);
                        driver.FindElementById("txt_telefone").SendKeys(applicationSettings.PresenterPhone);
                        driver.FindElementById("bt_prosseguir").SendKeys(" ");




                        if (File.Exists("c:\\temp\\Escritura.pdf"))
                            File.Delete("c:\\temp\\Escritura.pdf");
                        File.WriteAllBytes("c:\\temp\\Escritura.pdf", Convert.FromBase64String(requisition.ShipmentData.Base64Document));
                        Log.Information("Uploading Contract");

                        driver.FindElementById("uploadifive-file_upload").FindElements(OpenQA.Selenium.By.TagName("input"))[1].SendKeys("c:\\temp\\Escritura.pdf");

                        System.Threading.Thread.Sleep(5000);
                        for (int i = 0; i < 100; i++)
                        {
                            try
                            {
                                driver.FindElementById("txtDescricao").SendKeys(requisition.ShipmentData.Buyers.First().Name);
                                break;
                            }
                            catch (OpenQA.Selenium.NoSuchElementException ex)
                            {

                            }
                            System.Threading.Thread.Sleep(2000);
                        }


                        Log.Information("Saving");
                        driver.FindElementById("btnSalvar").SendKeys(" ");
                        for (int i = 0; i < 100; i++)
                        {
                            try
                            {
                                driver.FindElementById("btnProsseguir").SendKeys(" ");
                                break;
                            }
                            catch (OpenQA.Selenium.NoSuchElementException ex)
                            {

                            }
                            System.Threading.Thread.Sleep(2000);
                        }

                        Log.Information("Catching EProtocolo");
                        System.Diagnostics.Debug.WriteLine("teste: " + driver.FindElementById("lblTaxaPrenotacao").Text.Replace("R$ ", "").Replace(",", "."));

                        requisition.EPropocolo = new Model.EPropocolo();
                        requisition.EPropocolo.RegistryFees = Convert.ToDecimal(driver.FindElementById("lblTaxaPrenotacao").Text.Replace("R$ ", ""));
                        requisition.EPropocolo.ServiceValues = Convert.ToDecimal(driver.FindElementById("lblTaxaAdministracao").Text.Replace("R$ ", ""));
                        requisition.EPropocolo.TotalValues = Convert.ToDecimal(driver.FindElementById("lblValorTotal").Text.Replace("R$ ", ""));

                        //////for (int i = 0; i < 100; i++)
                        //////{
                        //////    try
                        //////    {
                        //////        chrome.FindElementById("btnProsseguir").SendKeys(" ");
                        //////        break;
                        //////    }
                        //////    catch (OpenQA.Selenium.NoSuchElementException ex)
                        //////    {

                        //////    }
                        //////    System.Threading.Thread.Sleep(2000);
                        //////}

                        //chrome.Navigate().GoToUrl("https://www.registradores.org.br/eProtocolo/PedidoFinalizadoAC.aspx?idc=763195&idpa=0");
                        //https://www.registradores.org.br/eProtocolo/PedidoFinalizadoAC.aspx?idc=763195&idpa=0
                        //////string protocolo = chrome.FindElementById("lblProtocolo").Text;
                        string protocolo = "Teste sem finalizar, por isso está sem Protocolo";
                        string urlConfirmation = driver.Url;
                        requisition.EPropocolo.Protocol = protocolo;


                        requisition.EPropocolo.ConfirmationScreen = s3.SaveFile(driver.GetScreenshot().AsBase64EncodedString, ".png");
                        requisition.EPropocolo.ConfirmationURL = urlConfirmation;
                        //////string idc = requisition.EPropocolo.ConfirmationURL.Substring(requisition.EPropocolo.ConfirmationURL.IndexOf("?"));

                        //////chrome.Navigate().GoToUrl("https://www.registradores.org.br/eProtocolo/frm_detalhes_contrato.aspx" + idc);
                        ////////https://www.registradores.org.br/eProtocolo/frm_detalhes_contrato.aspx?idc=763195&idpa=0
                        ////////requisition.EPropocolo.
                        //////requisition.EPropocolo.RequestData = chrome.FindElementByName("Form1").GetAttribute("innerHTML");
                        //////requisition.EPropocolo.RequestData = requisition.EPropocolo.RequestData.Remove(requisition.EPropocolo.RequestData.LastIndexOf("<span class=\"clear clean\"></span>"));
                        requisition.Status = Common.Enumerable.APIStatus.Success;

                        var ret = httpClientFW.Put<Model.Requisition, Model.Requisition>(new[] { "" }, requisition);

                        if (ret.ActionResult is Microsoft.AspNetCore.Mvc.OkResult)
                        {


                        }
                        /*
                         * renatojbussiere@gmail.com
                           04290429
                        */
                    }
                }
            Error:;
                Log.Information("Sleeping " + 60 * 1000 + " Seconds");
                System.Threading.Thread.Sleep(60 * 1000);
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
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return await Task.FromResult(base64ImageRepresentation);
        }
    }
}
