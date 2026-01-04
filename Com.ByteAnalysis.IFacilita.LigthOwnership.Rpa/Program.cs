using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.LigthOwnership.Rpa.ApiClient;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Com.ByteAnalysis.IFacilita.LightOwnership.Rpa
{
    static class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;
        private static Timer _timer;

        private async static Task<bool> CreateDriverAsync()
        {
            var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
            var service = ChromeDriverService.CreateDefaultService(pathDrive);
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.BinaryLocation = Configuration["ChromeBinary:Path"];

            var dirDownload = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
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
                var msg = $"[{DateTime.Now.ToString()}] - {message}";
                await File.AppendAllLinesAsync($"{DateTime.Now.ToString("dd-MM-yyyyHH")}.log", new string[] { msg });
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
            _timer = new Timer((500 * 30) * 1);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
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

        private async static Task<string> LoadCompleted(int timeout = 3)
        {
            var msg = await AlertIsPresent(timeout);
            if (!string.IsNullOrEmpty(msg))
                return await Task.FromResult(msg);

            try
            {
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
            catch { return await Task.FromResult("wait"); }
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                //Loader Configurations

                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                var apiService = new LigthOwnershipClient(Configuration);

                await WriteLog("Obtendo dados do servidor...");
                var ligthOwnershipResponse = await apiService.GetPendingAsync();

                if (ligthOwnershipResponse == null)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Total de registros encontrados: " + ligthOwnershipResponse.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                foreach (var owner in ligthOwnershipResponse)
                {
                    try
                    {
                        await WriteLog("Iniciando Driver Google Chrome");
                        await CreateDriverAsync();
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                        driver.Manage().Window.Maximize();

                        driver.Navigate().GoToUrl(Configuration["Urls:Base"]);

                        driver.FindElement(By.Name("Cliente.E_NOME")).Clear();
                        driver.FindElement(By.Name("Cliente.E_NOME")).SendKeys(owner.NameClient);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Cliente.E_PARTNER")).Clear();
                        driver.FindElement(By.Name("Cliente.E_PARTNER")).SendKeys(owner.CodeClient);
                        System.Threading.Thread.Sleep(50);

                        if (owner.PhysicalPerson != null)
                        {
                            driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/form/section[1]/section[1]/section/div[1]/div/label")).Click();
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_FILIACAO1")).Clear();
                            driver.FindElement(By.Name("Cliente.E_FILIACAO1")).SendKeys(owner.PhysicalPerson.Affiliation01);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_FILIACAO2")).Clear();
                            driver.FindElement(By.Name("Cliente.E_FILIACAO2")).SendKeys(owner.PhysicalPerson.Affiliation02);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_BIRTHDT")).Clear();
                            driver.FindElement(By.Name("Cliente.E_BIRTHDT")).SendKeys(owner.PhysicalPerson.BirthDate.ToString("dd/MM/yyyy"));
                            System.Threading.Thread.Sleep(50);

                            var selectHtml = new SelectElement(driver.FindElement(By.Name("Cliente.E_DOC_TYPE")));
                            selectHtml.SelectByValue(owner.PhysicalPerson.CodeDocumentType);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_DOC_NUMBER")).Clear();
                            driver.FindElement(By.Name("Cliente.E_DOC_NUMBER")).SendKeys(owner.PhysicalPerson.DocumentNumber);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_DOC_INSTITUTE")).Clear();
                            driver.FindElement(By.Name("Cliente.E_DOC_INSTITUTE")).SendKeys(owner.PhysicalPerson.IssuingBody);
                            System.Threading.Thread.Sleep(50);

                            var selectHtmlCountry = new SelectElement(driver.FindElement(By.Name("Cliente.E_DOC_COUNTRY")));
                            selectHtmlCountry.SelectByValue(owner.PhysicalPerson.CodeCountry);
                            System.Threading.Thread.Sleep(50);

                            var selectHtmlUf = new SelectElement(driver.FindElement(By.Name("Cliente.E_DOC_REGION")));
                            selectHtmlUf.SelectByValue(owner.PhysicalPerson.CodeUf);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_TAXNUM")).Clear();
                            driver.FindElement(By.Name("Cliente.E_TAXNUM")).SendKeys(owner.PhysicalPerson.Cpf);
                            System.Threading.Thread.Sleep(50);
                        }
                        else
                        {
                            driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/form/section[1]/section[1]/section/div[2]/div/label")).FindElement(By.TagName("span")).Click();
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_TAXNUM_CNPJ")).Clear();
                            driver.FindElement(By.Name("Cliente.E_TAXNUM_CNPJ")).SendKeys(owner.LegalPerson.Cnpj);
                            System.Threading.Thread.Sleep(50);

                            driver.FindElement(By.Name("Cliente.E_TAXNUM_SOCIO")).Clear();
                            driver.FindElement(By.Name("Cliente.E_TAXNUM_SOCIO")).SendKeys(owner.LegalPerson.CpfPartner);
                            System.Threading.Thread.Sleep(50);
                        }

                        driver.FindElement(By.Name("Cliente.E_TEL_NUMBER")).Clear();
                        driver.FindElement(By.Name("Cliente.E_TEL_NUMBER")).SendKeys(owner.Phone);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Cliente.E_CEL_NUMBER")).Clear();
                        driver.FindElement(By.Name("Cliente.E_CEL_NUMBER")).SendKeys(owner.CelPhone);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Cliente.E_SMTP_ADDR")).Clear();
                        driver.FindElement(By.Name("Cliente.E_SMTP_ADDR")).SendKeys(owner.Email);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.EnderecoCompleto")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.EnderecoCompleto")).SendKeys(owner.Address);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Numero")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Numero")).SendKeys(owner.Number);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Complemento")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Complemento")).SendKeys(owner.Complement);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Bairro")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Bairro")).SendKeys(owner.Neighborhood);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Cidade")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Cidade")).SendKeys(owner.City);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Estado")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Estado")).SendKeys(owner.Uf);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("Instalacao.Endereco.Cep")).Clear();
                        driver.FindElement(By.Name("Instalacao.Endereco.Cep")).SendKeys(owner.Cep);
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("DataAutoLeitura")).Clear();
                        driver.FindElement(By.Name("DataAutoLeitura")).SendKeys(owner.AutoReadingDate.ToString("dd/MM/yyyy"));
                        System.Threading.Thread.Sleep(50);

                        driver.FindElement(By.Name("LeituraMedidor")).Clear();
                        driver.FindElement(By.Name("LeituraMedidor")).SendKeys(owner.AutoReadingValue);
                        System.Threading.Thread.Sleep(50);

                        switch (owner.TypeActivity)
                        {
                            case Model.TypeActivity.Residential:
                                driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/form/section[3]/section/section/div[1]/div/label")).Click();
                                break;
                            case Model.TypeActivity.NoResidential:
                                driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/form/section[3]/section/section/div[2]/div/label")).Click();
                                break;
                            case Model.TypeActivity.ResidentialNoResidentialRural:
                                driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/form/section[3]/section/section/div[3]/div/label")).Click();
                                break;
                        }
                        System.Threading.Thread.Sleep(50);

                        var funcJS = @"function base64ToFile(base64, filename) {

                                        var arr = base64.split(','),
                                            mime = arr[0].match(/:(.*?);/)[1],
                                            bstr = atob(arr[1]), 
                                            n = bstr.length, 
                                            u8arr = new Uint8Array(n);
            
                                        while(n--){
                                            u8arr[n] = bstr.charCodeAt(n);
                                        }
        
                                        return new File([u8arr], filename, {type:mime});
                                };";

                        foreach (var arq in owner.Attachments)
                            js.ExecuteScript(funcJS + "; var dataTransfer = new DataTransfer();dataTransfer.items.add(base64ToFile('" + arq.FullBase64 + "','" + arq.FileName + "'));document.getElementById('dropzone-abertura-contrato').dispatchEvent(new DragEvent('drop', { dataTransfer: dataTransfer }));");

                        System.Threading.Thread.Sleep(50);

                        //wait reCaptcha
                        var countIntent = 0;
                        var solved = true;

                        while (driver.FindElement(By.TagName("body")).FindElement(By.ClassName("antigate_solver")).Text != "Solved")
                        {
                            if (countIntent >= (60 * 5))
                            {
                                solved = false;
                                break;
                            }
                            countIntent++;
                            System.Threading.Thread.Sleep(1000);
                            await WriteLog("Aguardando resolução do captcha. " + (countIntent + 1) + "s");
                        }

                        if (!solved)
                            continue;

                        driver.FindElement(By.Id("btnEnviar")).Click();

                        //Validação dos campos


                        try
                        {
                            System.Threading.Thread.Sleep(2000);
                            var errors = driver.FindElement(By.Id("msg-info"));
                            var fieldsErrors = errors.Text.Split("<br>", StringSplitOptions.RemoveEmptyEntries);
                            var fieldsSplited = fieldsErrors[0].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                            List<GlobalError> errorsLight = new List<GlobalError>();

                            int codeField = 0;
                            foreach (var msg in fieldsSplited)
                            {
                                var field = "General";
                                switch (msg)
                                {
                                    case "O campo Nome do Cliente deve ser preenchido.": codeField = 1; field = "NameClient"; break;
                                    case "Marque uma das opções de Tipo de Pessoa.": codeField = 2; field = "TypeClient"; break;
                                    case "O campo Data de Nascimento deve ser preenchido.": codeField = 3; field = "PhysicalPersonModel.BirthDate"; break;
                                    case "O campo Tipo do documento deve ser preenchido.": codeField = 4; field = "PhysicalPersonModel.CodeDocumentType"; break;
                                    case "O campo Nº do Documento deve ser preenchido.": codeField = 5; field = "PhysicalPersonModel.DocumentNumber"; break;
                                    case "O campo Órgão Emissor deve ser preenchido.": codeField = 6; field = "PhysicalPersonModel.IssuingBody"; break;
                                    case "O campo País deve ser preenchido.": codeField = 7; field = "PhysicalPersonModel.CodeCountry"; break;
                                    case "O campo UF deve ser preenchido.": codeField = 8; field = "PhysicalPersonModel.CodeUf"; break;
                                    case "O campo CPF deve ser preenchido.": codeField = 9; field = "PhysicalPersonModel.Cpf"; break;
                                    case "O campo CNPJ deve ser preenchido.": codeField = 10; field = "LegalPersonModel.Cnpj"; break;
                                    case "O campo CPF do sócio principal deve ser preenchido.": codeField = 11; field = "LegalPersonModel.CpfPartner"; break;
                                    case "O campo Endereço eletrônico (e-mail) deve ser preenchido.": codeField = 12; field = "Email"; break;
                                    case "O campo Endereço da Nova Unidade Consumidora deve ser preenchido.": codeField = 13; field = "Address"; break;
                                    case "O campo Número deve ser preenchido.": codeField = 14; field = "Number"; break;
                                    case "O campo Bairro deve ser preenchido.": codeField = 15; field = "Neighborhood"; break;
                                    case "O campo Município deve ser preenchido.": codeField = 16; field = "City"; break;
                                    case "O campo Estado deve ser preenchido.": codeField = 17; field = "Uf"; break;
                                    case "Marque uma das opções de Atividade exercida no local.": codeField = 18; field = "TypeActivity"; break;
                                    case "É obrigatório adicionar pelo menos um arquivo": codeField = 19; field = "Attachments"; break;
                                    case "É necessário clicar em \"Não sou um robô\".": codeField = 20; field = "AntiCaptcha"; break;

                                    default:
                                        break;
                                }

                                errorsLight.Add(new GlobalError()
                                {
                                    Code = codeField,
                                    Field = field,
                                    Message = msg
                                });
                            }

                            owner.Pending = false;
                            owner.Status = Common.Enumerable.APIStatus.Error;
                            owner.Errors = errorsLight;

                            await apiService.PutAsync(owner);
                            continue;

                        }
                        catch (Exception ex)
                        {
                            await WriteLog("Todos os campos estão válidos");
                        }
                        var loaded = await LoadCompleted();
                        while (loaded == "wait")
                        {
                            loaded = await LoadCompleted();
                            System.Threading.Thread.Sleep(50);
                        }

                        //Atualizar Cri
                        owner.Pending = false;
                        owner.Status = Common.Enumerable.APIStatus.Success;
                        await apiService.PutAsync(owner);
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

        static async Task Main(string[] args)
        {
            Console.Title = "[RPA] Light - Troca de Titularidade";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            await WriteLog("Iniciando RPA Light - Troca de Titularidade");
            await WriteLog("Ambiente de execução: " + environmentName);

            Configuration = new ConfigurationBuilder()
                      .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddCommandLine(args)
                      .Build();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            if (!Directory.Exists("download-captcha"))
                Directory.CreateDirectory("download-captcha");

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
