using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using System.Globalization;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Rpa.ApiClient;
using System.Net.Http;
using Com.ByteAnalysis.IFacilita.Common;
using System.Collections.Generic;
using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using Newtonsoft.Json;
using System.Text;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Rpa
{
    static class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;
        private static Timer _timer;

        private static IS3 s3;

        private async static Task<bool> CreateDriverAsync()
        {
            var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
            var service = ChromeDriverService.CreateDefaultService(pathDrive);
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.BinaryLocation = Configuration["ChromeBinary:Path"];

            string dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
            
            if (!Directory.Exists(dirDownload))
                Directory.CreateDirectory(dirDownload);

            options.AddUserProfilePreference("download.default_directory", dirDownload);
            options.AddUserProfilePreference("disable-popup-blocking", "true");

            var pathToExtension = Path.Combine(Configuration["ChromeBinary:PathBase"], "plugin", "anticaptcha-plugin_v0.49");
            options.AddArgument("load-extension=" + pathToExtension);

            driver = new ChromeDriver(service, options);
            return await Task.FromResult(true);
        }

        private async static Task<bool> WriteLog(string message)
        {
            try
            {
                if (!Directory.Exists(Configuration["OutputLogs:Path"]))
                    Directory.CreateDirectory(Configuration["OutputLogs:Path"]);

                var pathLog = $"{Configuration["OutputLogs:Path"]}/SearchProtest-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

                var msg = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}] - {message}";
                await File.AppendAllLinesAsync(pathLog, new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }

            return true;
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

        private async static Task<bool> StartTimer()
        {
            await DisposeTimerAsync();
            await WriteLog("Preparando timer para o ciclo de processamento.");
            _timer = new Timer(1000 * 60);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
        }

        private async static Task<bool> AlertIsPresent(int timeout = 3)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();

                return await Task.FromResult(true);
            }
            catch (NoAlertPresentException aEx)
            {
                return await Task.FromResult(false);
            }
        }

        private async static Task<bool> LoadCompleted(int timeout = 3)
        {
            //await AlertIsPresent(timeout);

            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeout));
                return await Task.FromResult(wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete"));
            }
            catch { return await Task.FromResult(false); }
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                //Loader Configurations

                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                var apiService = new SearchProtestClientApi(Configuration);

                await WriteLog("Obtendo dados do servidor...");
                var SearchProtestResponse = await apiService.GetPendingAsync();

                if (SearchProtestResponse == null)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Total de registros encontrados: " + SearchProtestResponse.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                const int timeWait = 500;

                foreach (var owner in SearchProtestResponse)
                {
                    await WriteLog("Processando requisição: " + owner.Id);
                    await WriteLog("Status atual da requisição: " + owner.StatusDownloadCertificates);

                    if (owner.StatusDownloadCertificates == "Unprocessed" && !owner.Approved)
                    {
                        try
                        {
                            await WriteLog("Iniciando Driver Google Chrome");
                            await CreateDriverAsync();
                            js = (IJavaScriptExecutor)driver;

                            var messageValidation = await Login(timeWait);

                            if (!string.IsNullOrEmpty(messageValidation))
                            {
                                List<GlobalError> errors = new List<GlobalError>();

                                if (owner.Errors.Count() > 0)
                                {
                                    errors = owner.Errors.ToList();
                                }

                                if (!messageValidation.Contains("Não foi possível conectar ao serviço de autenticação, aguarde alguns instantes e realize uma nova"))
                                {
                                    owner.Status = Common.Enumerable.APIStatus.Error;
                                    owner.Pending = false;
                                }
                                else
                                {
                                    owner.Pending = true;
                                }

                                errors.Add(new GlobalError() { Code = 0, Field = "Login", Message = messageValidation });
                                owner.Errors = errors;
                                owner.Updated = DateTime.Now;

                                await apiService.PutAsync(owner);
                                await WriteLog(messageValidation);

                                continue;
                            }

                            driver.Navigate().GoToUrl(Configuration["ProtestSp:UrlNothingContained"]);
                            while (driver.PageSource.Contains("Consulte grátis"))
                            {
                                await WriteLog("Aguardando load da página");
                                System.Threading.Thread.Sleep(timeWait);
                            }

                            while (!await LoadCompleted())
                                System.Threading.Thread.Sleep(timeWait);

                            //Tipo de documento
                            var selectHtmlDocumentType = new SelectElement(driver.FindElement(By.Id("TipoDocumento")));
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("TipoDocumento")).Location.Y});");

                            //Tipo de Documento
                            switch (owner.PersonType)
                            {
                                case PersonType.PHYSICAL:
                                    selectHtmlDocumentType.SelectByValue("1");
                                    break;
                                case PersonType.LEGAL:
                                    selectHtmlDocumentType.SelectByValue("2");
                                    break;
                                default:
                                    break;
                            }
                            System.Threading.Thread.Sleep(timeWait);

                            //Documento do Pesquisado
                            var documentFormat = UInt64.Parse(owner.DocumentPrincipal).ToString(@"000\.000\.000\-00");
                            driver.FindElement(By.Id("Documento")).SendKeys(documentFormat);
                            System.Threading.Thread.Sleep(timeWait);

                            //Botão consultar
                            driver.FindElement(By.XPath("//input[@VALUE='CONSULTAR']")).Click();
                            System.Threading.Thread.Sleep(timeWait * 2);

                            while (true)
                            {
                                try
                                {
                                    var divLoading = driver.FindElement(By.XPath("//div[@class='loading']"));
                                    if (!divLoading.GetAttribute("style").Contains("display: block;"))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        await WriteLog("Aguardando loading da página");
                                        System.Threading.Thread.Sleep(timeWait * 2);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await WriteLog("Falha ao tentar encontrar um elemento HTML." + ex.Message);
                                }
                            }

                            var spanHasProtest = driver.FindElement(By.XPath("//span[@class='labelTotalSP']")).Text;
                            if (spanHasProtest.Equals("Não constam protestos")) owner.HasProtest = false;
                            else owner.HasProtest = true;

                            Screenshot image = ((ITakesScreenshot)driver.FindElement(By.ClassName("content-filtro-listagem"))).GetScreenshot();
                            owner.UrlBillet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(image.AsBase64EncodedString, ".jpg");

                            owner.Pending = false;
                            owner.StatusDownloadCertificates = "Downloaded";

                            owner.Updated = DateTime.Now;
                            await apiService.PutAsync(owner);

                            await WriteLog("Requisição finalizada com sucesso. ID: " + owner.Id);
                        }
                        catch (Exception ex)
                        {
                            await WriteLog(ex.Message);
                        }
                        finally
                        {
                            await DisposeDriverAsync();
                        }
                    }
                    else if (owner.StatusDownloadCertificates == "Unprocessed" && owner.Approved)
                    {
                        try
                        {
                            var lastError = owner.Errors?.LastOrDefault();
                            if (lastError != null)
                            {
                                if (owner.Updated > DateTime.Now && lastError.Message.Contains("Serviço do Banco Bradesco Indisponível no momento. Tentar novamente mais tarde ou utilizar outro banco"))
                                {
                                    await WriteLog("Aguardando retorno do serviço de boletos");
                                    continue;
                                }
                            }

                            await WriteLog("Iniciando Driver Google Chrome");
                            await CreateDriverAsync();
                            js = (IJavaScriptExecutor)driver;

                            var messageValidation = await Login(timeWait);

                            if (!string.IsNullOrEmpty(messageValidation))
                            {
                                List<GlobalError> errors = new List<GlobalError>();
                                if (owner.Errors.Count() > 0)
                                {
                                    errors = owner.Errors.ToList();
                                }

                                if (!messageValidation.Contains("Não foi possível conectar ao serviço de autenticação, aguarde alguns instantes e realize uma nova"))
                                {
                                    owner.Status = Common.Enumerable.APIStatus.Error;
                                    owner.Pending = false;
                                }
                                else
                                {
                                    owner.Pending = true;
                                }

                                errors.Add(new GlobalError() { Code = 0, Field = "Login", Message = messageValidation });
                                owner.Errors = errors;
                                owner.Updated = DateTime.Now;

                                await apiService.PutAsync(owner);
                                await WriteLog(messageValidation);

                                continue;
                            }

                            driver.Navigate().GoToUrl(Configuration["ProtestSp:UrlProtest"]);
                            while (driver.PageSource.Contains("Consulte grátis"))
                            {
                                await WriteLog("Aguardando load da página");
                                System.Threading.Thread.Sleep(timeWait);
                            }

                            while (!await LoadCompleted())
                                System.Threading.Thread.Sleep(timeWait);

                            //Abrangência da pesquisa
                            switch (owner.Coverage)
                            {
                                case Model.Coverage.LAST5YEAR:
                                    driver.FindElement(By.Id("Abrangencia5")).Click();
                                    System.Threading.Thread.Sleep(timeWait);
                                    break;
                                case Model.Coverage.LAST10YEAR:
                                    driver.FindElement(By.Id("Abrangencia10")).Click();
                                    System.Threading.Thread.Sleep(timeWait);
                                    break;

                            }

                            //Forma de expedição
                            var selectHtmlCountry = new SelectElement(driver.FindElement(By.Id("TipoCertidao")));
                            switch (owner.Expedition)
                            {
                                case Model.Expedition.DIGITAL:
                                    selectHtmlCountry.SelectByValue("1");
                                    break;
                                case Model.Expedition.PAPER:
                                    selectHtmlCountry.SelectByValue("2");
                                    break;
                            }
                            System.Threading.Thread.Sleep(timeWait);

                            //Tipo de pessoa
                            var selectHtmlPersonType = new SelectElement(driver.FindElement(By.Id("TipoPessoa")));
                            switch (owner.PersonType)
                            {
                                case Model.PersonType.PHYSICAL:
                                    selectHtmlPersonType.SelectByValue("1");
                                    break;
                                case Model.PersonType.LEGAL:
                                    selectHtmlPersonType.SelectByValue("2");
                                    break;
                            }
                            System.Threading.Thread.Sleep(timeWait);

                            //Comarca
                            try
                            {
                                var selectHtmlJuridic = new SelectElement(driver.FindElement(By.Id("CodigoComarca")));
                                selectHtmlJuridic.SelectByValue(owner.JuridicalDistrictCode.ToString());
                                js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("CodigoComarca")).Location.Y});");
                                System.Threading.Thread.Sleep(timeWait);
                            }
                            catch (Exception ex)
                            {
                                var message = "A comarca informada não é válida. Consulte o endpoint api/Helper/JudicialDistrict para saber quais são os valores permitidos" + ex.Message;
                                owner.Status = Common.Enumerable.APIStatus.Error;
                                owner.Pending = false;
                                owner.Errors = new List<GlobalError>() {
                                    new GlobalError(){ Code = 3, Field = "juridicalDistrictCode", Message = message }
                                };

                                owner.Updated = DateTime.Now;
                                await apiService.PutAsync(owner);
                                await WriteLog(message);

                                continue;
                            }

                            //Todos Cartórios da Comarca
                            driver.FindElements(By.Name("SelecaoCartorios"))[0].Click();

                            //Nome do pesquisado
                            driver.FindElement(By.Id("PesquisadoNome")).SendKeys(owner.FullName);
                            System.Threading.Thread.Sleep(timeWait);

                            //Documento principal
                            var documentFormat = UInt64.Parse(owner.DocumentPrincipal).ToString(@"000\.000\.000\-00");
                            driver.FindElement(By.Id("PesquisadoDocumento")).SendKeys(documentFormat);
                            System.Threading.Thread.Sleep(timeWait);

                            //Tipo de documento
                            var selectHtmlDocument = new SelectElement(driver.FindElement(By.Id("DocumentoComplementarTipo")));
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("DocumentoComplementarTipo")).Location.Y});");
                            switch (owner.DocumentType)
                            {
                                case Model.DocumentType.RG:
                                    selectHtmlDocument.SelectByValue("1");
                                    break;
                                case Model.DocumentType.RGE:
                                    selectHtmlDocument.SelectByValue("2");
                                    break;
                            }
                            System.Threading.Thread.Sleep(timeWait);

                            //Documento complementar

                            driver.FindElement(By.Id("DocumentoComplementar")).SendKeys(owner.DocumentComplementary);
                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("DocumentoComplementar")).Location.Y});");
                            System.Threading.Thread.Sleep(timeWait);

                            driver.FindElement(By.XPath("/html/body/div[50]/div/div/div/div[2]/form/input[9]")).Click();
                            System.Threading.Thread.Sleep(timeWait);

                            try
                            {
                                var popupMessageError = driver.FindElement(By.Id("modal-erro-cenprot"));
                                var style = popupMessageError.GetAttribute("style").ToLower();
                                if (style.Contains("display: block"))
                                {
                                    List<GlobalError> errors = new List<GlobalError>();

                                    var divAlert = popupMessageError.FindElement(By.ClassName("modal-mensagem-alerta"));
                                    var messageAlerty = divAlert.FindElement(By.TagName("p"));
                                    messageValidation = messageAlerty.Text;
                                    switch (messageValidation)
                                    {
                                        case "O documento informado não é um CPF válido!":
                                            errors.Add(new GlobalError() { Code = 5, Field = "documentPrincipal", Message = messageAlerty.Text });
                                            break;
                                        case "Para prosseguir é necessário informar todos os dados da solicitação de certidão.":
                                            errors.Add(new GlobalError() { Code = 0, Field = "Form", Message = messageAlerty.Text });
                                            break;
                                        default:
                                            break;
                                    }

                                    try
                                    {
                                        var fieldsInvalids = driver.FindElements(By.ClassName("borderRed"));
                                        var fieldName = string.Empty;
                                        var fieldCode = 0;

                                        foreach (var field in fieldsInvalids)
                                        {
                                            var id = field.GetAttribute("id");
                                            var placeHolder = field.GetAttribute("placeholder");
                                            switch (id)
                                            {
                                                case "TipoCertidao": fieldName = "expedition"; fieldCode = 1; break;
                                                case "TipoPessoa": fieldName = "personType"; fieldCode = 2; break;
                                                case "CodigoComarca": fieldName = "juridicalDistrictCode"; fieldCode = 3; break;
                                                case "PesquisadoNome": fieldName = "fullName"; fieldCode = 4; break;
                                                case "PesquisadoDocumento": fieldName = "documentPrincipal"; fieldCode = 5; break;
                                                case "DocumentoComplementarTipo": fieldName = "documentType"; fieldCode = 6; break;
                                                case "DocumentoComplementar": fieldName = "documentComplementary"; fieldCode = 7; break;
                                                default:
                                                    break;
                                            }

                                            messageValidation = "Para prosseguir é necessário informar todos os dados da solicitação de certidão.";
                                            errors.Add(new GlobalError() { Code = fieldCode, Field = fieldName, Message = $"{placeHolder} - {messageValidation}" });
                                        }
                                    }
                                    catch { }

                                    owner.Status = Common.Enumerable.APIStatus.Error;
                                    owner.Pending = false;
                                    owner.Errors = errors;

                                    owner.Updated = DateTime.Now;
                                    await apiService.PutAsync(owner);
                                    await WriteLog(messageValidation);

                                    continue;
                                }

                            }
                            catch { }

                            driver.FindElement(By.XPath("/html/body/div[18]/div/div/div[3]/button[2]")).Click();
                            System.Threading.Thread.Sleep(timeWait);

                            driver.FindElement(By.XPath("/html[1]/body[1]/div[25]/div[1]/div[1]/div[3]/button[1]")).Click();
                            while (!await LoadCompleted())
                                System.Threading.Thread.Sleep(timeWait);

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.XPath("/html/body/div[51]/div/div[2]/div/div/ul/li[2]/a")).Location.Y});");
                            driver.FindElement(By.XPath("/html/body/div[51]/div/div[2]/div/div/ul/li[2]/a")).Click();
                            System.Threading.Thread.Sleep(timeWait);

                            driver.FindElement(By.XPath("/html/body/div[18]/div/div/div[3]/button[2]")).Click();
                            System.Threading.Thread.Sleep(timeWait);

                            try
                            {
                                var billetAllowed = driver.FindElement(By.Id("modal-erro-cenprot"));
                                var billetAllowedProperties = billetAllowed.GetAttribute("style");

                                if (billetAllowedProperties.Contains("display: block"))
                                {
                                    List<GlobalError> errors = new List<GlobalError>();
                                    if (owner.Errors != null)
                                        errors = owner.Errors.ToList();

                                    var divAlert = billetAllowed.FindElement(By.ClassName("modal-mensagem-alerta"));
                                    var messageAlerty = divAlert.FindElement(By.TagName("p"));
                                    messageValidation = messageAlerty.Text;

                                    if (messageValidation.Contains("Serviço do Banco Bradesco Indisponível no momento. Tentar novamente mais tarde ou utilizar outro banco"))
                                    {
                                        if (owner.Updated < DateTime.Now)
                                            errors.Add(new GlobalError() { Code = 0, Field = "Form", Message = messageValidation });

                                        owner.Updated = DateTime.Now.AddMinutes(5);
                                    }
                                    else
                                    {
                                        errors.Add(new GlobalError() { Code = 0, Field = "Form", Message = messageValidation });
                                    }
                                    owner.Updated = DateTime.Now;
                                    owner.Status = Common.Enumerable.APIStatus.Pending;
                                    owner.Pending = true;
                                    owner.Errors = errors;

                                    owner.Updated = DateTime.Now;
                                    await apiService.PutAsync(owner);
                                    await WriteLog(messageValidation);

                                    continue;
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                            owner.OrderNumber = driver.FindElement(By.XPath("/html/body/div[50]/div/div/div/div/div[2]/div/p/span[1]/strong")).Text;

                            js.ExecuteScript($" window.scrollBy(0,{driver.FindElement(By.Id("btn-Boleto")).Location.Y});");
                            driver.FindElement(By.Id("btn-Boleto")).Click();
                            System.Threading.Thread.Sleep(timeWait);

                            await WriteLog("Iniciando download do boleto");

                            var urlBillet = driver.FindElement(By.Id("aSegundaVia")).GetAttribute("href");
                            driver.Navigate().GoToUrl(urlBillet);

                            while (!await LoadCompleted())
                                System.Threading.Thread.Sleep(timeWait);

                            using (HttpClient httpClient = new HttpClient())
                            {
                                try
                                {
                                    var result = await httpClient.GetAsync(urlBillet);
                                    using (Stream streamToReadFrom = await result.Content.ReadAsStreamAsync())
                                    {
                                        owner.UrlBillet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(Convert.ToBase64String(await result.Content.ReadAsByteArrayAsync()), ".pdf");

                                        var outputDir = "Boletos";
                                        if (!Directory.Exists(outputDir))
                                            Directory.CreateDirectory(outputDir);

                                        var fileName = Guid.NewGuid().ToString("N");
                                        await WriteLog("Gravando boleto no disco: " + $"{fileName}.pdf");

                                        using (Stream streamToWriteTo = File.Open(Path.Combine(outputDir, $"{fileName}.pdf"), FileMode.Create))
                                        {
                                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                        }

                                        owner.Pending = true;
                                        owner.StatusDownloadCertificates = "Waiting";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    owner.Pending = true;
                                    owner.StatusDownloadCertificates = "Unprocessed";

                                    await WriteLog("Houve uma falha ao tentar baixar o boleto. " + ex.Message);

                                }
                            }

                            owner.Updated = DateTime.Now;
                            await apiService.PutAsync(owner);

                        }
                        catch (Exception ex)
                        {
                            await WriteLog(ex.Message);
                        }
                        finally
                        {
                            await DisposeDriverAsync();
                        }
                    }
                    else if (owner.StatusDownloadCertificates == "Waiting")
                    {
                        try
                        {
                            var lastUpdate = DateTime.Now - owner.Updated;

                            if (lastUpdate.Value.Days == 0)
                                continue;

                            await WriteLog("Iniciando Driver Google Chrome");
                            await CreateDriverAsync();
                            js = (IJavaScriptExecutor)driver;

                            var messageValidation = await Login(timeWait);
                            if (!string.IsNullOrEmpty(messageValidation))
                            {
                                List<GlobalError> errors = new List<GlobalError>();
                                if (owner.Errors.Count() > 0)
                                {
                                    errors = owner.Errors.ToList();
                                }

                                if (!messageValidation.Contains("Não foi possível conectar ao serviço de autenticação, aguarde alguns instantes e realize uma nova"))
                                {
                                    owner.Status = Common.Enumerable.APIStatus.Error;
                                    owner.Pending = false;
                                }
                                else
                                {
                                    owner.Pending = true;
                                }

                                errors.Add(new GlobalError() { Code = 0, Field = "Login", Message = messageValidation });
                                owner.Errors = errors;
                                owner.Updated = DateTime.Now;

                                await apiService.PutAsync(owner);
                                await WriteLog(messageValidation);

                                continue;
                            }

                            driver.Navigate().GoToUrl(Configuration["ProtestSp:UrlList"]);
                            while (driver.PageSource.Contains("Consulte grátis"))
                            {
                                await WriteLog("Aguardando load da página");
                                System.Threading.Thread.Sleep(timeWait);
                            }

                            while (!await LoadCompleted())
                                System.Threading.Thread.Sleep(timeWait);

                            driver.FindElement(By.Id("ProtocoloPedido")).SendKeys(owner.OrderNumber);
                            System.Threading.Thread.Sleep(timeWait);

                            driver.FindElement(By.XPath("/html/body/div[50]/div/div/div/div/div[1]/form/input[13]")).Click();
                            System.Threading.Thread.Sleep(timeWait * 3);

                            var table = driver.FindElement(By.Id("tblCertidoesListagem"));
                            var tbody = table.FindElement(By.TagName("tbody"));
                            var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
                            try
                            {
                                var trs = tbody.FindElements(By.TagName("tr"));
                                owner.Certificates = new List<Certificate>();
                                int countIntent = 0;
                                bool existsPdf = false;

                                foreach (var tr in trs)
                                {
                                    while (countIntent < 5)
                                    {
                                        try
                                        {
                                            var tds = tr.FindElements(By.TagName("td"));
                                            var registry = tds[3].Text;
                                            var dataLink = tds[8].FindElements(By.TagName("a"));

                                            if (dataLink == null || dataLink.Count() <= 0)
                                            {
                                                countIntent = 5;
                                                break;
                                            }

                                            js.ExecuteScript($" window.scrollBy(0,{dataLink[0].Location.Y});");
                                            System.Threading.Thread.Sleep(timeWait);

                                            dataLink[0].Click();
                                            System.Threading.Thread.Sleep(timeWait);

                                            var file = Directory.GetFiles(dirDownload);
                                            var fileReaded = File.ReadAllBytes(file[0]);
                                            var fileBase64 = Convert.ToBase64String(fileReaded);
                                            var certificate = new Certificate();

                                            certificate.Registry = registry;
                                            certificate.UrlCertificatePdf = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(fileBase64, ".pdf");

                                            File.Delete(file[0]);

                                            dataLink[1].Click();
                                            System.Threading.Thread.Sleep(timeWait);

                                            file = Directory.GetFiles(dirDownload);
                                            fileReaded = File.ReadAllBytes(file[0]);
                                            fileBase64 = Convert.ToBase64String(fileReaded);
                                            certificate.UtlCertificateP7s = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(fileBase64, ".p7s");
                                            File.Delete(file[0]);
                                            owner.Certificates.Add(certificate);
                                            existsPdf = true;
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            await WriteLog(ex.Message);
                                            await WriteLog("Tentando novamente");

                                        }
                                    }

                                    #region MyRegion
                                    /*
                                    using (HttpClient httpClient = new HttpClient())
                                    {
                                        dataLink[0].Click();

                                        var dataPostPdf = dataLink[0].GetAttribute("onclick");
                                        dataPostPdf = dataPostPdf.Replace("AcaoCertidao(", "").Replace(")", "");
                                        var dataPostParts = dataPostPdf.Split(',');
                                        var content = new StringContent(JsonConvert.SerializeObject(new { ChaveDownload = dataPostParts[0].Trim(), Status = Convert.ToInt32(dataPostParts[1].Trim()), TotalDeclaracoes = 0 }), Encoding.UTF8, "application/json");
                                        var result = httpClient.PostAsync("https://www.protestosp.com.br/certidao-de-protesto/VerificarCertidaoDigital", content).Result;

                                        using (Stream streamToReadFrom = await result.Content.ReadAsStreamAsync())
                                        {
                                            var resultResponse = await result.Content.ReadAsStringAsync();
                                            owner.UrlBillet = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(Convert.ToBase64String(await result.Content.ReadAsByteArrayAsync()), ".pdf");

                                            var outputDir = "Boletos";
                                            if (!Directory.Exists(outputDir))
                                                Directory.CreateDirectory(outputDir);

                                            var fileName = Guid.NewGuid().ToString("N");
                                            using (Stream streamToWriteTo = File.Open(Path.Combine(outputDir, $"{fileName}.pdf"), FileMode.Create))
                                            {
                                                await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                            }
                                        }
                                    } */
                                    #endregion

                                }

                                if (existsPdf)
                                {
                                    owner.Pending = false;
                                    owner.StatusDownloadCertificates = "Downloaded";

                                }

                                owner.Updated = DateTime.Now;
                                await apiService.PutAsync(owner);
                            }
                            catch (Exception ex)
                            {
                                owner.Pending = true;
                                owner.StatusDownloadCertificates = "Waiting";
                                owner.Updated = DateTime.Now;
                                await apiService.PutAsync(owner);
                            }

                        }
                        catch (Exception ex)
                        {
                            await WriteLog(ex.Message);
                        }
                        finally
                        {
                            await DisposeDriverAsync();
                        }
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

        private static async Task<string> Login(int timeWait = 5)
        {
            driver.Manage().Window.Maximize();

            //fazer o login
            driver.Navigate().GoToUrl(Configuration["ProtestSp:UrlLogin"]);
            while (!await LoadCompleted())
                System.Threading.Thread.Sleep(timeWait);

            #region Login
            driver.FindElement(By.Name("login")).SendKeys(Configuration["ProtestSp:UserLogin"]);
            System.Threading.Thread.Sleep(timeWait);

            driver.FindElement(By.Name("senha")).SendKeys(Configuration["ProtestSp:PasswordLogin"]);
            System.Threading.Thread.Sleep(timeWait);

            driver.FindElement(By.XPath("/html/body/section/div[2]/div[2]/div/form/div/div[3]/button")).Click();
            System.Threading.Thread.Sleep(timeWait);

            try
            {
                var popupMessageError = driver.FindElement(By.Id("modal-erro-cenprot"));
                var style = popupMessageError.GetAttribute("style").ToLower();
                if (style.Contains("display: block"))
                {
                    return await Task.FromResult("Autenticação inválida, caso não possua cadastro, clique em 'Primeiro Acesso'.");
                }

            }
            catch { }

            while (driver.PageSource.Contains("Caso já possua cadastro, acesse ao lado por meio de login/e-"))
            {
                await WriteLog("Aguardando authenticação na página");
                System.Threading.Thread.Sleep(timeWait);
            }

            while (!await LoadCompleted())
                System.Threading.Thread.Sleep(timeWait);

            #endregion

            return await Task.FromResult("");
        }

        static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            await WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] SearchProtest", "Pesquisa de Protestos");

            await WriteLog("Iniciando RPA SearchProtest - Pesquisa de Protestos");
            await WriteLog("Ambiente de execução: " + environmentName);

            Console.Title = "[RPA] SearchProtest - Pesquisa de Protestos";

            s3 = new Common.Impl.S3();

            await ExecuteProcess();

            Console.Read();
        }

        private async static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            await DisposeTimerAsync();
            await DisposeDriverAsync();
        }
    }
}
