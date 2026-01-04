using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.Common.Impl;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RoboAntiCaptchaDomain.Interfaces;
using RoboAntiCaptchaExternalService;
using RoboAntiCaptchaExternalService.Interfaces;
using RoboAntiCaptchaModel.MapperConfig;
using RoboAntiCaptchaModel.Request;
using RoboAntiCaptchaModel.Response;
using RoboAntiCaptchaService.Configs;
using RoboAntiCaptchaService.Process;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace RoboAntiCaptchaBillet
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;

        private static Timer _timer;
        private static ConfigBilletMapper _configs;
        private static IS3 s3;

        static async Task Main(string[] args)
        {
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
                var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

                Configuration = new ConfigurationBuilder()
                      .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddCommandLine(args)
                      .Build();

                Console.Title = "[RPA] - ITBI->[BOLETO] - Imposto sobre a transmissão de bens imóveis";
                LocalLog.WriteLogStart(Configuration, "[RPA] [ITBI]->[BOLETO]", "Imposto sobre a transmissão de bens imóveis");

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                var serviceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddSingleton<IConfigMapper, ConfigBilletMapper>()
                    .AddSingleton<IFacilitaClient, FacilitaClient>()
                    .BuildServiceProvider();

                //Cria o diretório de download de captcha
                if (!Directory.Exists("download-captcha"))
                    Directory.CreateDirectory("download-captcha");

                //Loader Configurations
                var configMapper = serviceProvider.GetService<IConfigMapper>();
                await configMapper.LoadMappersAsync(Configuration);
                _configs = (configMapper as ConfigBilletMapper);

                s3 = new S3();

                await ExecuteProcess();

                Console.Read();
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

        private async static Task<bool> StartTimer()
        {
            await DisposeTimerAsync();
            await WriteLog("Preparando timer para o ciclo de processamento.");
            _timer = new Timer((1000 * 30) * 1);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                var apiService = new FacilitaClient(_configs.ApiConfig);

                await WriteLog("Obtendo dados do servidor...");
                var 
                    
                    desRequests = await apiService.GetPendingGuideAsync();

                if (guidesRequests == null)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Iniciando Driver Google Chrome");
                await CreateDriverAsync();

                await WriteLog("Total de registros encontrados: " + guidesRequests.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                foreach (var guide in guidesRequests)
                {
                    try
                    {
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl(_configs.UrlsMapper.Base);

                        //Página inicial
                        await WriteLog("Carregando página inicial");

                        var itbi2 = new ITBI2GuiasEmitidas()
                        {
                            ConsultaPor = "1",
                            CpfCnpjAdquirente = guide.Protocol.PurchaserDocument,
                            InscricaoImobiliaria = guide.Iptu.ToString(),
                            NumeroGuiaProtocolo = guide.Protocol.ProtocolNumber.ToString(),
                            Captcha = "",
                        };

                        var itbi2Proc = new ProcessServiceBase<ITBI2GuiasEmitidasMapperConfig, ITBI2GuiasEmitidas>(_configs.iTBI2Config, itbi2, driver);
                        await itbi2Proc.ExecuteWrite();

                        //Consulta
                        var itbi2Response = new ITBI2GuiasEmitidasResponse();
                        var itbi2ResponseProc = new ProcessServiceBase<ITBI2GuiasEmitidasResponseMapperConfig, ITBI2GuiasEmitidasResponse>(_configs.iTBI2ResponseConfig, itbi2Response, driver);
                        await itbi2ResponseProc.ExecuteRead(false);

                        DateTime? allowedPayment = null;
                        if (!string.IsNullOrEmpty(itbi2Response.DisponivelParaPagamento))
                            allowedPayment = Convert.ToDateTime(itbi2Response.DisponivelParaPagamento, cultureInfo);

                        var billetUrl = guide.Guide.BilletBase64;

                        guide.Guide = new RoboAntiCaptchaModel.ExternalService.Guide
                        {
                            BilletBase64 = billetUrl,
                            AllowedPayment = allowedPayment,
                            BaseCalculation = Convert.ToDecimal(itbi2Response.BaseCalculo, cultureInfo),
                            DueDate = Convert.ToDateTime(itbi2Response.DataVencimento, cultureInfo),
                            GuideNumber = itbi2Response.NumeroGuia,
                            Iptu = itbi2Response.InscricaoImovel,
                            ProtocolNumber = itbi2Response.NumeroProtocolo,
                            PurchaserDocument = itbi2Response.CpfCgcAdquirente,
                            TotalValue = Convert.ToDecimal(itbi2Response.ValorTotalAPagar, cultureInfo),
                            Value = Convert.ToDecimal(itbi2Response.ValorDeclarado, cultureInfo),
                            ValueMora = Convert.ToDecimal(itbi2Response.ValorMora, cultureInfo),
                            ValuePenalty = Convert.ToDecimal(itbi2Response.ValorMulta, cultureInfo),
                            ValueTax = Convert.ToDecimal(itbi2Response.ValorImposto, cultureInfo),
                        };

                        await WriteLog("Atualizando informações da Guia de requisição id: " + guide.Id);
                        await apiService.PutAsync(guide);
                        await WriteLog("Guia finalizada com sucesso.");

                        if (string.IsNullOrEmpty(guide.Guide.BilletBase64) && guide.Guide.GuideNumber == "NÃO CONSTA O PAGAMENTO DESTA GUIA")
                        {
                            await WriteLog("Boleto disponível para impressão");

                            driver.Navigate().GoToUrl(Configuration["Urls:Billet"]);

                            var itbi2ImpGuiasEmitidas = new Itbi2ImpGuiasEmitidas()
                            {
                                Captcha = "",
                                Document = guide.Generation.Purchaser,
                                Iptu = guide.Iptu.ToString(),
                                Protocol = guide.Guide.ProtocolNumber
                            };

                            var itbi2ImpGuiasEmitidasProc = new ProcessServiceBase<Itbi2ImpGuiasEmitidasMapperConfig, Itbi2ImpGuiasEmitidas>(_configs.Itbi2ImpGuiasEmitidasConfig, itbi2ImpGuiasEmitidas, driver);
                            await itbi2ImpGuiasEmitidasProc.ExecuteWrite();

                            var currentUrl = driver.Url;
                            using var clientHandler = new System.Net.Http.HttpClientHandler();
                            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                            using (HttpClient httpClient = new HttpClient(clientHandler))
                            {
                                var result = await httpClient.GetAsync(currentUrl);
                                if (!result.IsSuccessStatusCode)
                                {
                                    await WriteLog("Não foi possível baixar o boleto.");
                                    await WriteLog(await result.Content.ReadAsStringAsync());
                                    continue;
                                }

                                if (result.Content.Headers.ContentType.MediaType != "application/pdf")
                                {
                                    await WriteLog("Não foi possível baixar o boleto.");
                                    await WriteLog("Formato do arquivod diferente do esperado");
                                    continue;
                                }

                                using (Stream streamToReadFrom = await result.Content.ReadAsStreamAsync())
                                {
                                    guide.Guide.BilletBase64 = "https://ifacilita.s3.us-east-2.amazonaws.com/" + s3.SaveFile(Convert.ToBase64String(await result.Content.ReadAsByteArrayAsync()), ".pdf");

                                    var outputDir = "Billets";
                                    if (!Directory.Exists(outputDir))
                                        Directory.CreateDirectory(outputDir);

                                    var fileName =guide.Iptu + "." + Guid.NewGuid().ToString("N");
                                    using (Stream streamToWriteTo = File.Open(Path.Combine(outputDir, $"{fileName}.pdf"), FileMode.Create))
                                    {
                                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                    }
                                }

                                guide.StatusGuide = 2;
                                guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Success;
                                if (!string.IsNullOrEmpty(guide.UrlCallback))
                                {
                                    //Fazer o postback
                                    guide.UrlCallbackResponse = "Callback não implementado";
                                }
                            }

                            await apiService.PutAsync(guide);
                            await WriteLog("Boleto finalizado com sucesso.");
                        }

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

        private async static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            await DisposeTimerAsync();
            await DisposeDriverAsync();
        }

        private static System.Timers.Timer _timerAwait;
        private async static Task<bool> WriteLog(string message)
        {
            try
            {
                if (_timerAwait == null)
                {
                    var myExe = Directory.GetCurrentDirectory() + "/" + "RoboAntiCaptchaBillet.exe";
                    _timerAwait = new Timer(1000 * 10);
                    _timerAwait.Elapsed += (s, e) => { System.Diagnostics.Process.Start(myExe); System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); };
                    _timerAwait.Start();
                }

                var msg = $"[{DateTime.Now.ToString()}] - {message}";
                await File.AppendAllLinesAsync($"{DateTime.Now.ToString("dd-MM-yyyyHH")}.log", new string[] { msg });
                await Console.Out.WriteLineAsync(msg);
            }
            catch { }
            finally
            {
                if (_timerAwait != null)
                {
                    _timerAwait.Stop();
                    _timerAwait.Dispose();
                    _timerAwait = null;
                }
            }

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

        private async static Task<bool> CreateDriverAsync()
        {
            var service = ChromeDriverService.CreateDefaultService($"drivers/{Configuration["ChromeBinary:Version"]}");
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.BinaryLocation = Configuration["ChromeBinary:Path"];

            driver = new ChromeDriver(service, options);
            return await Task.FromResult(true);
        }
    }
}
