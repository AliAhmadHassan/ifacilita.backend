using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using Com.ByteAnalysis.IFacilita.CertificateESajSp.Rpa.ApiClient;
using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Rpa
{
    static class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;
        private static Timer _timer;
        private static IS3 s3;

        private static CertificatedApiClient apiService = null;

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

                var pathLog = $"{Configuration["OutputLogs:Path"]}/CertificateESajSp-{DateTime.Now.ToString("dd-MM-yyyy")}.log";

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

                var msg = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")}] - [{key}] - DefectsDefined -> Defeitos Ajuizados";
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
            _timer = new Timer((500 * 30) * 1);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
        }

        private async static Task<bool> AlertIsPresent(int timeout = 3)
        {
            try
            {
                await WriteLog("Verificando se existem 'Alert' abertos");
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
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeout));
                return await Task.FromResult(wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete"));
            }
            catch
            {
                await WriteLog("Aguardando carregamento da página");
                return await Task.FromResult(false);
            }
        }

        static async Task Main(string[] args)
        {
            Console.Title = "[RPA] DefectsDefined - Defeitos Ajuizados";
            var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
            var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

            Configuration = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            await WriteLog("Chave de Inicialização: " + envStart);
            LocalLog.WriteLogStart(Configuration, "[RPA] DefectsDefined", "Defeitos Ajuizados");

            await WriteLog("Iniciando RPA DefectsDefined - Defeitos Ajuizados");
            await WriteLog("Ambiente de execução: " + environmentName);

            s3 = new Common.Impl.S3();

            apiService = new CertificatedApiClient(Configuration);
            await ExecuteProcess();

            Console.Read();
        }

        private async static Task<bool> ExistRequisition(ResumeOrderModel cert)
        {
            await WriteLog("Obtendo dados do cadastro anterior");
            var currentRequest = await apiService.GetCurrentAsync(cert.Cpf, DateTime.Now);

            if (currentRequest != null)
            {
                await WriteLog("Registro encontrado. Id: " + currentRequest.Id);
                cert.DataOrder = currentRequest.DataOrder;

                if (string.IsNullOrEmpty(currentRequest.DataOrder?.UrlCertificate))
                {
                    await WriteLog("Certificado ainda não foi baixado. Verificando a disponibilidade.");

                    if (!await DownloadCertificate(currentRequest))
                    {
                        await WriteLog("O Certificado ainda não está disponível");
                        cert.Status = Common.Enumerable.APIStatus.Pending;
                        cert.OrderStatus = OrderStatus.WaitCertificate;
                        cert.Pending = true;
                    }
                    else
                    {
                        await WriteLog("Certificado baixado com sucesso");
                        currentRequest.DataOrder = cert.DataOrder;

                        await WriteLog("Atualizando a requisição anterior");
                        await apiService.PutAsync(currentRequest);
                    }
                }
                else
                {
                    await WriteLog("Atualizando dados da requisição atual com os dados da requisição anterior. Atual: " + cert.Id + ", Anterior: " + currentRequest.Id);

                    cert.Status = Common.Enumerable.APIStatus.Success;
                    cert.OrderStatus = OrderStatus.Finish;
                    cert.Pending = false;
                    
                }

                await apiService.PutAsync(cert);
                return true;
            }
            else
            {
                var msgError = "Já foi cadastrado um pedido de certidão para este documento";
                cert.Status = Common.Enumerable.APIStatus.Error;
                cert.OrderStatus = OrderStatus.Finish;
                cert.Pending = false;
                cert.Errors = new List<GlobalError>() {
                    new GlobalError(){ Code = 2, Field = "cpf", Message = msgError }
                };

                await apiService.PutAsync(cert);
                await WriteLog(msgError);
                await WriteLog("Seguindo para a próxima requisição");

                return false;
            }
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                await WriteLog("Obtendo dados do servidor...");
                var certificateRequests = await apiService.GetPendingAsync();

                if (certificateRequests == null)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Total de registros encontrados: " + certificateRequests.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                const int timeoutWait = 500;

                foreach (var cert in certificateRequests)
                {
                    try
                    {
                        js = (IJavaScriptExecutor)driver;

                        if (cert.OrderStatus == Model.OrderStatus.Generated)
                        {
                            await WriteLog("Iniciando Driver Google Chrome");
                            await CreateDriverAsync();

                            driver.Manage().Window.Maximize();
                            driver.Navigate().GoToUrl(Configuration["Urls:Base"]);

                            var selectHtml = new SelectElement(driver.FindElement(By.Id("cdModelo")));
                            selectHtml.SelectByValue(cert.ModelCode.ToString());
                            System.Threading.Thread.Sleep(timeoutWait);

                            switch (cert.PersonType)
                            {
                                case Model.PersonType.Physical:
                                    driver.FindElement(By.Id("tpPessoaF")).Click();
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("nmCadastroF")).Clear();
                                    driver.FindElement(By.Id("nmCadastroF")).SendKeys(cert.FullName);
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("identity.nuCpfFormatado")).Clear();
                                    driver.FindElement(By.Id("identity.nuCpfFormatado")).SendKeys(cert.Cpf);
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("identity.nuRgFormatado")).Clear();
                                    driver.FindElement(By.Id("identity.nuRgFormatado")).SendKeys(cert.Rg);
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    switch (cert.GenderType)
                                    {
                                        case Model.GenderType.Male:
                                            driver.FindElement(By.Id("flGeneroM")).Click();
                                            break;
                                        case Model.GenderType.Female:
                                            driver.FindElement(By.Id("flGeneroF")).Click();
                                            break;
                                    }
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("identity.solicitante.deEmail")).Clear();
                                    driver.FindElement(By.Id("identity.solicitante.deEmail")).SendKeys(cert.Email);
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    break;
                                case Model.PersonType.Legal:
                                    driver.FindElement(By.Id("tpPessoaJ")).Click();
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("nmCadastroJ")).Clear();
                                    driver.FindElement(By.Id("nmCadastroJ")).SendKeys(cert.FullName);
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    driver.FindElement(By.Id("identity.nuCnpjFormatado")).Clear();
                                    driver.FindElement(By.Id("identity.nuCnpjFormatado")).SendKeys(cert.Cpf);
                                    System.Threading.Thread.Sleep(timeoutWait);
                                    break;
                            }

                            driver.FindElement(By.Id("confirmacaoInformacoes")).Click();
                            System.Threading.Thread.Sleep(timeoutWait);

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
                            }

                            if (!solved)
                                continue;

                            driver.FindElement(By.Id("pbEnviar")).Click();

                            try
                            {
                                IWebElement element = null;

                                await WriteLog("Verificando a validação da página");

                                try
                                {
                                    element = driver.FindElement(By.Id("mensagemAlert"));
                                }
                                catch
                                {
                                    await WriteLog("O Id mensagemAlert não foi encontrado");
                                    System.Threading.Thread.Sleep(timeoutWait);
                                }



                                try
                                {
                                    if (element == null)
                                        element = driver.FindElement(By.XPath("//div[@class='modalConteudo']/p/span"));
                                }
                                catch
                                {
                                    await WriteLog("O xPath '/html/body/div[5]/div/div[2]/p/span' não foi encontrado");
                                    System.Threading.Thread.Sleep(timeoutWait);

                                    var msgError = "Já foi cadastrado um pedido de certidão para este";
                                    if (driver.PageSource.Contains(msgError))
                                    {
                                        _ = ExistRequisition(cert);
                                        continue;
                                    }
                                    else
                                    {
                                        await WriteLog("Essa página não contem a mensagem de erro esperada. Seguindo com o fluxo");
                                    }
                                }

                                if (element == null)
                                {
                                    await WriteLog("A página não apresentou erros de validação. Seguindo com o fluxo");
                                }
                                else
                                {
                                    var msg = element.Text;
                                    var fieldName = string.Empty;
                                    var fieldCode = 0;

                                    switch (msg)
                                    {
                                        case "O campo 'Nome Completo' deve ser preenchido.":
                                            fieldName = "fullName";
                                            fieldCode = 1;
                                            break;

                                        case "O campo 'CPF' deve ser preenchido.":
                                            fieldName = "cpf";
                                            fieldCode = 2;
                                            break;

                                        case "O campo 'RG' deve ser preenchido.":
                                            fieldName = "rg";
                                            fieldCode = 3;
                                            break;

                                        case "O campo 'E-Mail' deve ser preenchido.":
                                            fieldName = "email";
                                            fieldCode = 4;
                                            break;

                                        default:

                                            if (msg.Contains("O CPF digitado não é válido. Valor digitado:"))
                                            {
                                                fieldName = "cpf";
                                                fieldCode = 2;
                                            }
                                            else if (msg.Contains("O e-mail informado não é válido. Valor digitado:"))
                                            {
                                                fieldName = "email";
                                                fieldCode = 4;
                                            }
                                            else if (msg.Contains("Já foi cadastrado um pedido de certidão para este"))
                                            {
                                                _ = ExistRequisition(cert);

                                                fieldName = "cpf";
                                                fieldCode = 2;
                                            }
                                            else
                                            {
                                                fieldName = "General";
                                                fieldCode = 0;
                                            }

                                            break;
                                    }

                                    cert.Status = Common.Enumerable.APIStatus.Error;
                                    cert.OrderStatus = OrderStatus.Finish;
                                    cert.Pending = false;
                                    cert.Errors = new List<GlobalError>() {
                                        new GlobalError(){ Code = fieldCode, Field = fieldName, Message = msg }
                                    };

                                    await apiService.PutAsync(cert);
                                    await WriteLog(msg);
                                    continue;
                                }

                            }
                            catch (Exception ex)
                            {
                                await WriteLog(ex.Message);
                                await WriteLog("A página não apresentou erros de validação. Seguindo com o fluxo");
                            }

                            int timeoutLoadPage = 1;
                            while (!await LoadCompleted())
                            {
                                await WriteLog("Aguardando carregamento da página.");
                                System.Threading.Thread.Sleep(timeoutWait);
                                if (timeoutLoadPage == 10)
                                    break;
                                else
                                    timeoutLoadPage++;
                            }

                            await WriteLog("Aguardando página ser carregada");

                            int countLoad = 1;
                            var pageIsload = true;

                            while (!driver.PageSource.Contains("Dados para Emissão da Certidão"))
                            {
                                if (countLoad >= 5)
                                {
                                    var msgError = "A página não carregou corretamente. A requisição voltará para a fila";

                                    cert.Status = Common.Enumerable.APIStatus.Pending;
                                    cert.OrderStatus = OrderStatus.Generated;
                                    cert.Pending = true;

                                    var listError = cert.Errors?.ToList();

                                    if (listError == null)
                                        listError = new List<GlobalError>();

                                    listError.Add(new GlobalError() { Code = 0, Field = "General", Message = msgError });

                                    cert.Errors = listError;

                                    await apiService.PutAsync(cert);
                                    await WriteLog(msgError);
                                    pageIsload = false;

                                    break;
                                }
                                else
                                {
                                    await WriteLog("Aguardando página ser carregada");
                                    System.Threading.Thread.Sleep(timeoutWait);
                                    countLoad++;
                                }
                            }

                            if (!pageIsload)
                                continue;

                            cert.DataOrder = new Model.DataOrderModel()
                            {
                                NumberOrder = driver.FindElement(By.XPath("/html/body/table[4]/tbody/tr/td/form/div[1]/table[2]/tbody/tr[1]/td[2]/span")).Text,
                                DateOrder = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/table[4]/tbody/tr/td/form/div[1]/table[2]/tbody/tr[2]/td[2]/span")).Text, cultureInfo),
                                UrlCertificate = null
                            };

                            cert.OrderStatus = OrderStatus.WaitCertificate;
                            if (await DownloadCertificate(cert))
                            {
                                cert.OrderStatus = OrderStatus.Finish;
                                cert.Pending = false;
                            }

                        }
                        else if (cert.OrderStatus == OrderStatus.WaitCertificate)
                        {
                            await WriteLog("Verificando a disponibilidade para download");
                            if (cert.DataOrder == null)
                            {
                                List<GlobalError> errors = new List<GlobalError>();
                                if (cert.Errors != null)
                                    errors = cert.Errors.ToList();

                                errors.Add(new GlobalError()
                                {
                                    Code = 0,
                                    Field = "DataOrder",
                                    Message = "Não foram encontrados os dados do pedido para realizar o download do certificado. a Requisição será reprocessada."
                                });

                                cert.Status = Common.Enumerable.APIStatus.Error;
                                cert.Pending = true;
                                cert.OrderStatus = OrderStatus.Generated;
                            }
                            else
                            {
                                if (await DownloadCertificate(cert))
                                {
                                    cert.OrderStatus = OrderStatus.Finish;
                                    cert.Pending = false;
                                }
                                else
                                {
                                    await WriteLog($"O download do certificado ainda não está disponível. Id: {cert.Id}, Número do Pedido:{cert.DataOrder.NumberOrder}, Data: {cert.DataOrder.DateOrder.ToString("dd/MM/yyyy HH.mm.ss")}");
                                }
                            }

                        }

                        await apiService.PutAsync(cert);
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

        private static async Task<bool> DownloadCertificate(ResumeOrderModel orderModel)
        {
            try
            {
                await WriteLog("Iniciando tentativa de download da certidão");

                using var clientHandler = new System.Net.Http.HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient httpClient = new HttpClient(clientHandler))
                {
                    var result = await httpClient.GetAsync(string.Format(Configuration["Urls:Download"], orderModel.DataOrder.NumberOrder, orderModel.DataOrder.DateOrder.ToString("dd/MM/yyyy"), orderModel.PersonType == PersonType.Physical ? "F" : "J", orderModel.Cpf));
                    if (!result.IsSuccessStatusCode) return await Task.FromResult(false);

                    if (result.Content.Headers.ContentType.MediaType != "application/pdf") return await Task.FromResult(false);
                    using (Stream streamToReadFrom = await result.Content.ReadAsStreamAsync())
                    {
                        orderModel.DataOrder.UrlCertificate = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(Convert.ToBase64String(await result.Content.ReadAsByteArrayAsync()), ".pdf");

                        var outputDir = "Certidoes";
                        if (!Directory.Exists(outputDir))
                            Directory.CreateDirectory(outputDir);

                        var fileName = Guid.NewGuid().ToString("N");
                        using (Stream streamToWriteTo = File.Open(Path.Combine(outputDir, $"{fileName}.pdf"), FileMode.Create))
                        {
                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        }
                    }
                }

                await WriteLog("Certidão baixada com sucesso. Id: " + orderModel.Id);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                await WriteLog("Houve um erro na tentativa de baixar a certidão: " + ex.Message);
            }

            return await Task.FromResult(false);
        }

        private async static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            await DisposeTimerAsync();
            await DisposeDriverAsync();
        }
    }
}
