using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Com.ByteAnalysis.IFacilita.Mcri.Model;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Attributes;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs;
using Com.ByteAnalysis.IFacilita.Mcri.Rpa.RpaProcess;
using Com.ByteAnalysis.IFacilita.Mcri.Service.Mapper;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RoboAntiCaptchaExternalService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa
{
    static class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;
        private static EnumMapperAcquisitionTitleType enumMapperAcquisitionTitleType;
        private static EnumMapperPropertyFractionType enumMapperPropertyFractionType;
        private static EnumMapperTypeOfContributor enumMapperTypeOfContributor;

        private static Timer _timer;

        private async static Task<bool> StartTimer()
        {
            await DisposeTimerAsync();
            await WriteLog("Preparando timer para o ciclo de processamento.");
            _timer = new Timer((1000 * 30) * 1);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
        }

        private async static Task<bool> CreateDriverAsync()
        {
            var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
            var service = ChromeDriverService.CreateDefaultService(pathDrive);
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.BinaryLocation = Configuration["ChromeBinary:Path"];

            driver = new ChromeDriver(service, options);
            return await Task.FromResult(true);
        }

        private async static Task<string> GetHtmlElementAsync(PropertyInfo property)
        {
            var htmlElement = "";
            foreach (MapperConfigAttributes attr in property.GetCustomAttributes(true))
                htmlElement = attr.HtmlElementType;

            return await Task.FromResult(htmlElement);
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                //Loader Configurations
                var _configs = new MapperConfig();
                await _configs.LoadMappersAsync(Configuration);

                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                var apiService = new CriClient(_configs.ApiConfig);

                await WriteLog("Obtendo dados do servidor...");
                var criResponse = await apiService.GetPendingAsync();

                if (criResponse == null || criResponse.Count() <= 0)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Total de registros encontrados: " + criResponse.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                foreach (var cri in criResponse)
                {
                    try
                    {
                        if (cri.Iptu == null)
                        {
                            cri.Status = Common.Enumerable.APIStatus.Error;
                            cri.Pending = false;
                            cri.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = "Iptu", Message = "Campo iptu é obrigatório" } };
                            await apiService.PutAsync(cri);
                            continue;
                        }


                        await WriteLog("Iniciando Driver Google Chrome");
                        await CreateDriverAsync();
                        driver.Manage().Window.Maximize();

                        driver.Navigate().GoToUrl(_configs.UrlsMapper.Base);

                        var login = new
                        {
                            Captcha = "",
                            Iptu = cri.Iptu
                        };

                        var procLogin = new ProcessServiceBase<PageLoginMapper, object>(_configs.PageLoginMapper, login, driver);
                        var respLogin = await procLogin.ExecuteWrite();
                        if (respLogin.Message != "")
                        {
                            cri.Status = Common.Enumerable.APIStatus.Error;
                            cri.Pending = false;
                            cri.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = respLogin.Field, Message = respLogin.Message } };
                            await apiService.PutAsync(cri);
                            continue;
                        }

                        //Opção
                        if (cri.NewImmobile)
                        {
                            var newImmobile = driver.FindElements(By.Name(_configs.PageFormMapper.NovoImovel));
                            foreach (var item in newImmobile)
                            {
                                item.Click();

                                if (item.GetAttribute("value") == "0")
                                {
                                    item.Click();
                                    driver.FindElement(By.Name(_configs.PageFormMapper.NovoImovelEndereco)).SendKeys(cri.AddressNewImmobile);
                                }

                            }
                        }

                        //Adquirente(s)
                        var countP = cri.Purchasers.Count();

                        driver.FindElement(By.Name(_configs.PageFormMapper.QtdeAdquirente)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.QtdeAdquirente)).SendKeys(countP.ToString());

                        driver.FindElement(By.XPath(_configs.PageFormMapper.AdquirenteExibir)).Click();

                        for (int i = 1; i <= countP; i++)
                        {
                            driver.FindElement(By.Name($"{_configs.PageFormMapper.AdquirenteNomeCompleto}{i}")).Clear();
                            driver.FindElement(By.Name($"{_configs.PageFormMapper.AdquirenteNomeCompleto}{i}")).SendKeys(cri.Purchasers.ToArray()[i - 1].FullName);

                            driver.FindElement(By.Name($"{_configs.PageFormMapper.AdquirenteDocumento}{i}")).Clear();
                            driver.FindElement(By.Name($"{_configs.PageFormMapper.AdquirenteDocumento}{i}")).SendKeys(cri.Purchasers.ToArray()[i - 1].Document);

                            var selectHtml = new SelectElement(driver.FindElement(By.Name($"{_configs.PageFormMapper.AdquirenteTipocontribuinte}{i}")));
                            selectHtml.SelectByText(enumMapperTypeOfContributor.GetText(cri.Purchasers.ToArray()[i - 1].TypeOfContributor));
                        }

                        //Transmitente(s)
                        var countT = cri.Transmittings.Count();

                        driver.FindElement(By.Name(_configs.PageFormMapper.QtdeTransmitente)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.QtdeTransmitente)).SendKeys(countT.ToString());

                        driver.FindElement(By.XPath(_configs.PageFormMapper.TransmitenteExibir)).Click();

                        for (int i = 1; i <= countT; i++)
                        {
                            driver.FindElement(By.Name($"{_configs.PageFormMapper.TransmitenteNomeCompleto}{i}")).Clear();
                            driver.FindElement(By.Name($"{_configs.PageFormMapper.TransmitenteNomeCompleto}{i}")).SendKeys(cri.Transmittings.ToArray()[i - 1].FullName);
                        }

                        //Título de Aquisição
                        var selectDocument = new SelectElement(driver.FindElement(By.Name($"{_configs.PageFormMapper.Documento}")));
                        selectDocument.SelectByText(enumMapperAcquisitionTitleType.GetText(cri.AcquisitionTitle.Document));

                        driver.FindElement(By.Name(_configs.PageFormMapper.Cartorio)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.Cartorio)).SendKeys(cri.AcquisitionTitle.Registry);

                        driver.FindElement(By.Name(_configs.PageFormMapper.Oficio)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.Oficio)).SendKeys(cri.AcquisitionTitle.Trade);

                        driver.FindElement(By.Name(_configs.PageFormMapper.Livro)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.Livro)).SendKeys(cri.AcquisitionTitle.Book);

                        driver.FindElement(By.Name(_configs.PageFormMapper.Folha)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.Folha)).SendKeys(cri.AcquisitionTitle.Leaf);

                        driver.FindElement(By.Name(_configs.PageFormMapper.Data)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.Data)).SendKeys(cri.AcquisitionTitle.Date);

                        driver.FindElement(By.Name(_configs.PageFormMapper.ValorTransacao)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.ValorTransacao)).SendKeys(cri.AcquisitionTitle.Value.ToString());

                        driver.FindElement(By.Name(_configs.PageFormMapper.ImpostoTransmicao)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.ImpostoTransmicao)).SendKeys(cri.AcquisitionTitle.TransferTax.ToString());

                        driver.FindElement(By.Name(_configs.PageFormMapper.NumeroGuia)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.NumeroGuia)).SendKeys(cri.AcquisitionTitle.GuideNumber.ToString());

                        //FRAÇÕES - Consulte a AJUDA
                        switch (cri.Fraction.PropertyFraction)
                        {
                            case PropertyFractionType.Decimal:
                                //frcimoveldec
                                driver.FindElement(By.Id($"{_configs.PageFormMapper.FracaoImovelTipo}1")).Click();
                                driver.FindElement(By.Name("frcimoveldec")).Clear();
                                driver.FindElement(By.Name("frcimoveldec")).SendKeys(cri.Fraction.FractionValue);

                                break;
                            case PropertyFractionType.Fraction:
                                driver.FindElement(By.Id($"{_configs.PageFormMapper.FracaoImovelTipo}3")).Click();
                                //frcimovelfracn
                                driver.FindElement(By.Name("frcimovelfracn")).Clear();
                                driver.FindElement(By.Name("frcimovelfracn")).SendKeys(cri.Fraction.FractionValue.Split('/')[0]);
                                //frcimovelfracd
                                driver.FindElement(By.Name("frcimovelfracd")).Clear();
                                driver.FindElement(By.Name("frcimovelfracd")).SendKeys(cri.Fraction.FractionValue.Split('/')[1]);
                                break;
                            case PropertyFractionType.Percent:
                                //frcimovelperc
                                driver.FindElement(By.Id($"{_configs.PageFormMapper.FracaoImovelTipo}2")).Click();

                                driver.FindElement(By.Name("frcimovelperc")).Clear();
                                driver.FindElement(By.Name("frcimovelperc")).SendKeys(cri.Fraction.FractionValue);

                                break;
                            default:
                                break;
                        }

                        driver.FindElement(By.Name(_configs.PageFormMapper.FracaoImovel)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.FracaoImovel)).SendKeys(cri.Fraction.FractionIdeal.ToString());

                        //Dados para Entrega da Guia de IPTU
                        driver.FindElement(By.Name(_configs.PageFormMapper.NomeEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.NomeEntrega)).SendKeys(cri.DeliveryGuide.FullName);

                        driver.FindElement(By.Name(_configs.PageFormMapper.DocumentoEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.DocumentoEntrega)).SendKeys(cri.DeliveryGuide.Document);

                        driver.FindElement(By.Name(_configs.PageFormMapper.LogradouroEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.LogradouroEntrega)).SendKeys(cri.DeliveryGuide.Address);

                        driver.FindElement(By.Name(_configs.PageFormMapper.NumeroEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.NumeroEntrega)).SendKeys(cri.DeliveryGuide.Number.ToString());

                        driver.FindElement(By.Name(_configs.PageFormMapper.ComplementoEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.ComplementoEntrega)).SendKeys(cri.DeliveryGuide.Complement);

                        driver.FindElement(By.Name(_configs.PageFormMapper.NomeDeclaranteEntrega)).Clear();
                        driver.FindElement(By.Name(_configs.PageFormMapper.NomeDeclaranteEntrega)).SendKeys(cri.DeliveryGuide.NameDeclarant);

                        //Dados Logradouro
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript($"document.getElementsByName('{_configs.PageFormMapper.CodeLogradouroEntrega}').value = '{cri.DeliveryGuide.CodeAddress}';");
                        js.ExecuteScript($"document.getElementsByName('{_configs.PageFormMapper.Logradouro}').value = '{cri.DeliveryGuide.Address}';");

                        js.ExecuteScript($"document.getElementById('{_configs.PageFormMapper.CodeLogradouroEntrega}').setAttribute('value','{cri.DeliveryGuide.CodeAddress}');");
                        js.ExecuteScript($"document.getElementById('{_configs.PageFormMapper.Logradouro}').setAttribute('value', '{cri.DeliveryGuide.Address}');");

                        //Enviar Requisição
                        driver.FindElement(By.XPath(_configs.PageFormMapper.Submeter)).Click();
                        var msgReq = await LoadCompleted();

                        while (msgReq == "wait")
                        {
                            System.Threading.Thread.Sleep(250);
                            msgReq = await LoadCompleted();
                        }

                        if (msgReq != "wait")
                        {
                            cri.Status = Common.Enumerable.APIStatus.Error;
                            cri.Pending = false;

                            var allMsgs = msgReq.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                            List<GlobalError> errors = new List<GlobalError>();

                            foreach (var msg in allMsgs)
                            {
                                var msgClear = msg.Replace("\n", "").Replace("\r", "");
                                if (string.IsNullOrEmpty(msgClear))
                                    continue;

                                var fieldName = string.Empty;
                                switch (msgClear)
                                {
                                    case "Informe o nome do Declarante.": fieldName = "DeliveryGuideIptuModel.NameDeclarant"; break;
                                    case "Informe o Núm.Porta do destinatário para entrega da Guia de IPTU.": fieldName = "DeliveryGuideIptuModel.Number"; break;
                                    case "Selecione um Cód. de Logradouro e Logradouro (Dados para Entrega da Guia de IPTU).": fieldName = "DeliveryGuideIptuModel.CodeAddress"; break;
                                    case "Informe o CPF/CNPJ do destinatário para entrega da Guia de IPTU.": fieldName = "DeliveryGuideIptuModel.Document"; break;
                                    case "Informe o nome do destinatário para entrega da Guia de IPTU.": fieldName = "DeliveryGuideIptuModel.FullName"; break;
                                    case "Informe a fração do Terreno correspondente ao imóvel.": fieldName = "FractionModel.FractionIdeal"; break;
                                    case "Informe a fração do Imóvel adquirido pelo Título.": fieldName = "FractionModel.FractionValue"; break;
                                    case "Informe a Data do Título.": fieldName = "AcquisitionTitle.Date"; break;
                                    case "Selecione um Documento.": fieldName = "AcquisitionTitle.AcquisitionTitleType"; break;
                                    case "Informe a Quantidade de Transmitentes.": fieldName = "TransmittingModel"; break;
                                    case "Informe a Quantidade de Adquirentes.": fieldName = "PurchaserModel"; break;
                                    case "Selecione uma opção: Confirmar/Informar um novo endereço do imóvel.": fieldName = "AddressNewImmobile"; break;
                                    default:
                                        break;
                                }

                                errors.Add(new GlobalError() { Code = 0, Field = fieldName, Message = msgClear });
                            }

                            cri.Errors = errors;
                            await apiService.PutAsync(cri);
                            continue;
                        }

                        //Confirmar
                        driver.FindElement(By.Name(_configs.PageFormMapper.Confirmar)).Click();
                        while (await LoadCompleted() != "wait")
                            System.Threading.Thread.Sleep(250);

                        //Primeiro Alerta
                        await AlertIsPresent();
                        while (await LoadCompleted() != "wait")
                            System.Threading.Thread.Sleep(250);

                        //Segunda Alerta
                        await AlertIsPresent();
                        while (await LoadCompleted() != "wait")
                            System.Threading.Thread.Sleep(250);

                        //Obter dados do Protocolo
                        var protocolGeneration = new ProtocolModel();
                        protocolGeneration.DateGeneration = Convert.ToDateTime(driver.FindElement(By.XPath(_configs.PageFormMapper.DataGeracao)).Text, cultureInfo);
                        protocolGeneration.Due = Convert.ToDateTime(driver.FindElement(By.XPath(_configs.PageFormMapper.Vencimento)).Text, cultureInfo);
                        protocolGeneration.Protocol = driver.FindElement(By.XPath(_configs.PageFormMapper.Protocolo)).Text;

                        cri.Protocol = protocolGeneration;
                        cri.Pending = false;

                        //Atualizar Cri
                        await apiService.PutAsync(cri);
                    }
                    catch (Exception ex)
                    {
                        await WriteLog(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                await WriteLog("Falha na aplicação: " + ex.Message);
            }
            finally
            {
                await DisposeDriverAsync();
                await StartTimer();
                await WriteLog("Ciclo de processamento finalizado com sucesso.");
            }
        }

        private async static Task<string> LoadCompleted(int timeout = 3)
        {
            var msg = await AlertIsPresent(timeout);
            if (!string.IsNullOrEmpty(msg))
                return await Task.FromResult(msg);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeout));
            var loaded = await Task.FromResult(wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete"));

            if (!loaded)
            {
                return await Task.FromResult("wait");
            }
            else
            {
                return await Task.FromResult("");
            }
        }

        private async static Task<bool> DisposeDriverAsync()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
                driver.Dispose();
                driver = null;
            }

            return await Task.FromResult(true);
        }

        private async static Task<bool> DisposeTimerAsync()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            return await Task.FromResult(true);
        }

        private async static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            await DisposeTimerAsync();
            await DisposeDriverAsync();
        }

        private async static Task<bool> WriteLog(string message)
        {
            try
            {
                var msg = $"[{DateTime.Now.ToString()}] - {message}";
                await File.AppendAllLinesAsync($"{DateTime.Now.ToString("dd-MM-yyyyHH")}.log", new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }

            return true;
        }

        static async Task Main(string[] args)
        {
            Console.Title = "[RPA] - [MCRI] Comunicação de Alteração de Titularidade";

            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

            await WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] - [MCRI]", "Comunicação de Alteração de Titularidade");
            await WriteLog("Iniciando RPA SearchProtest - Pesquisa de Protestos");
            await WriteLog("Ambiente de execução: " + environmentName);

            //Cria o diretório de download de captcha
            if (!Directory.Exists("download-captcha"))
                Directory.CreateDirectory("download-captcha");

            enumMapperAcquisitionTitleType = new EnumMapperAcquisitionTitleType();
            enumMapperPropertyFractionType = new EnumMapperPropertyFractionType();
            enumMapperTypeOfContributor = new EnumMapperTypeOfContributor();

            await ExecuteProcess();

            Console.Read();
        }

        private async static Task<string> AlertIsPresent(int timeout = 3)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
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
    }
}
