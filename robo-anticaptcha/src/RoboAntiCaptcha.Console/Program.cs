using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.Common.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RoboAntiCaptcha.Model.Response;
using RoboAntiCaptchaDomain.Interfaces;
using RoboAntiCaptchaExternalService;
using RoboAntiCaptchaExternalService.Interfaces;
using RoboAntiCaptchaModel.Enums;
using RoboAntiCaptchaModel.MapperConfig;
using RoboAntiCaptchaModel.Request;
using RoboAntiCaptchaModel.Response;
using RoboAntiCaptchaService.Configs;
using RoboAntiCaptchaService.Process;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace RoboAntiCaptchaConsole
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static IWebDriver driver;

        private static Timer _timer;
        private static ConfigMapper _configs;

        static async Task Main(string[] args)
        {
            try
            {
                Console.Title = "[RPA] - ITBI";

                var environmentName = CurrentEnvironment();// Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
                var envStart = Environment.GetEnvironmentVariable("IFACILITASTART", EnvironmentVariableTarget.User);

                Configuration = new ConfigurationBuilder()
                      .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"C:\\iFacilita\\Dependences\\appsettings.chrome.{environmentName}.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddCommandLine(args)
                      .Build();

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                var serviceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddSingleton<IConfigMapper, ConfigMapper>()
                    .AddSingleton<IFacilitaClient, FacilitaClient>()
                    .BuildServiceProvider();

                Console.Title = "[RPA] - ITBI - Imposto sobre a transmissão de bens imóveis";
                LocalLog.WriteLogStart(Configuration, "[RPA] ITBI", "Imposto sobre a transmissão de bens imóveis");

                //Cria o diretório de download de captcha
                if (!Directory.Exists("download-captcha"))
                    Directory.CreateDirectory("download-captcha");

                //Loader Configurations
                var configMapper = serviceProvider.GetService<IConfigMapper>();
                await configMapper.LoadMappersAsync(Configuration);
                _configs = (configMapper as ConfigMapper);

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

        static string CurrentEnvironment()
        {
            string env = string.Empty;
            try
            {
                Console.Out.WriteLine("Obtendo valor da variável de ambiente DOTNETCORE_ENVIRONMENT para EnvironmentVariableTarget.User");

                env = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);
                Console.Out.WriteLine("Valor encontrado: " + env);

                if (string.IsNullOrEmpty(env))
                {
                    Console.Out.WriteLine("Obtendo valor da variável de ambiente DOTNETCORE_ENVIRONMENT para EnvironmentVariableTarget.Machine");
                    env = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);

                    if (string.IsNullOrEmpty(env))
                    {
                        env = "Development";
                        Console.Out.WriteLine("Valor encontrado: " + env);
                    }
                    else
                        Console.Out.WriteLine("Valor encontrado: " + env);
                }

                if (string.IsNullOrEmpty(env))
                {
                    Console.Out.WriteLine("Obtendo valor da variável de ambiente DOTNETCORE_ENVIRONMENT para EnvironmentVariableTarget.Process");
                    env = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Process);
                    Console.Out.WriteLine("Valor encontrado: " + env);
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Não foi possível obter a variável de ambiente. " + ex.Message);
                env = "Production";
            }

            return env;
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
            var pathDrive = Path.Combine(Configuration["ChromeBinary:PathBase"], "drivers", Configuration["ChromeBinary:Version"]);
            var service = ChromeDriverService.CreateDefaultService(pathDrive);
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.BinaryLocation = Configuration["ChromeBinary:Path"];

            driver = new ChromeDriver(service, options);
            return await Task.FromResult(true);
        }

        private async static Task ExecuteProcess()
        {
            try
            {
                await DisposeTimerAsync();
                await WriteLog("Iniciando a execução do processo.");

                var apiService = new FacilitaClient(_configs.ApiConfig);

                await WriteLog("Obtendo dados do servidor...");
                var guidesRequests = await apiService.GetPendingAsync();

                if (guidesRequests == null || guidesRequests.Count() <= 0)
                {
                    await WriteLog("Nenhum registro foi encontrado");
                    await StartTimer();
                    return;
                }

                await WriteLog("Total de registros encontrados: " + guidesRequests.Count());
                var cultureInfo = new CultureInfo("pt-BR");

                foreach (var guide in guidesRequests)
                {
                    try
                    {
                        await WriteLog("Iniciando Driver Google Chrome");
                        await CreateDriverAsync();
                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl(_configs.UrlsMapper.Base);

                        //Loader Home Page
                        var homePage = new ProcessServiceBase<SolicitacaoGuiaMapperConfig, object>(_configs.SolicitacaoGuiaMapper, new object(), driver);
                        var result = await homePage.Submit();

                        await WriteLog("Carregando EntSimulacao");

                        //Loader EntSimulacao
                        var entSimulacao = new EntSimulacao()
                        {
                            Captcha = "",
                            Iptu = guide.Iptu.ToString(),
                            NaturezaOperacao = Convert.ToInt32(guide.TransactionNature).ToString("00"),
                            Pal = guide.Pal,
                            ParteTransferida = guide.TransferredPart,
                            ValorDeclarado = (Math.Round(Convert.ToDecimal(guide.Value.ToString(), cultureInfo), 2)).ToString()
                        };

                        var procEntSimu = new ProcessServiceBase<EntSimulacaoMapperConfig, EntSimulacao>(_configs.EntSimulacaoMapper, entSimulacao, driver);
                        var responseEntSimulacao = await procEntSimu.ExecuteWrite();
                        if (responseEntSimulacao.Message != "")
                        {
                            await WriteLog(responseEntSimulacao.Message + "\n" + JsonConvert.SerializeObject(entSimulacao));
                            guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Error;
                            guide.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = responseEntSimulacao.Field, Message = responseEntSimulacao.Message } };
                            await apiService.PutAsync(guide);
                            continue;
                        }

                        //Loader Simular
                        var simular = new Simular() { };
                        var responseSimular = new ProcessServiceBase<SimularMapperConfig, Simular>(_configs.SimularMapper, simular, driver);



                        if (guide.Preview)
                            await responseSimular.ExecuteRead(false);
                        else
                            await responseSimular.ExecuteRead(true);

                        guide.Simulate = new RoboAntiCaptchaModel.ExternalService.Simulate()
                        {
                            Address = simular.Endereco,
                            CalculationBasis = Convert.ToDecimal(simular.BaseCalculo, cultureInfo),
                            Due = Convert.ToDateTime(simular.Vencimento, cultureInfo),
                            Iptu = long.Parse(simular.Iptu),
                            Pal = simular.Pal,
                            Taxation = Convert.ToDecimal(simular.Imposto, cultureInfo),
                            TransactionNature = (TransactionNature)EnumsValues.TransactionNatureDescriptionToValue[simular.NaturezaOperacao],
                            TransferredPart = simular.PercentualTransferido,
                            Utilization = simular.Utilizacao,
                            Value = Convert.ToDecimal(simular.ValorDeclarado, cultureInfo)
                        };



                        if (guide.Preview)
                        {
                            guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Success;

                            await WriteLog("Atualizando informações da Guia de requisição id: " + guide.Id);
                            await apiService.PutAsync(guide);
                            await WriteLog("Guia finalizada com sucesso.");
                            continue;
                        }

                        //Loader AvisoSolicitacao
                        var avisoSolicitacao = new AvisoSolicitacao() { };
                        var responseAvisoSolicitacao = new ProcessServiceBase<AvisoSolicitacaoMapperConfig, AvisoSolicitacao>(_configs.AvisoSolicitacaoMapper, avisoSolicitacao, driver);
                        await responseAvisoSolicitacao.ExecuteRead();

                        //Loader EntGeracao
                        await WriteLog("Carregando EntGeracao");
                        var entGeracao = new EntGeracao()
                        {
                            Adquirente = guide.Generation.Purchaser,
                            Transmitente = guide.Generation.Transmitted
                        };
                        var requestEntGeracao = new ProcessServiceBase<EntiGeracaoMapperConfig, EntGeracao>(_configs.EntiGeracaoMapper, entGeracao, driver);
                        var respEntGeracao = await requestEntGeracao.ExecuteWrite();
                        if (respEntGeracao.Message != "")
                        {
                            await WriteLog(respEntGeracao.Message + "\n" + JsonConvert.SerializeObject(entGeracao));
                            guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Error;
                            guide.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = respEntGeracao.Field, Message = respEntGeracao.Message } };
                            await apiService.PutAsync(guide);
                            continue;
                        }

                        //Loader BuscaAdqCed
                        await WriteLog("Carregando BuscaAdqCed");
                        var buscaAdqCed = new BuscaAdqCed()
                        {
                            Adquirente = Convert.ToInt32(guide.PurchaserTransmitted.PurchaserOwnerSettings).ToString(),
                            AreaLazer = guide.PurchaserTransmitted.RecreationArea ? "S" : "N",
                            Bairro = guide.PurchaserTransmitted.Neighborhood,
                            BanheiroEmpregada = guide.PurchaserTransmitted.BathroomMaid ? "S" : "N",
                            Cep = guide.PurchaserTransmitted.Cep,
                            Cidade = guide.PurchaserTransmitted.City,
                            Complemento = guide.PurchaserTransmitted.Complement,
                            Ddd = guide.PurchaserTransmitted.Ddd.ToString(),
                            Elevador = guide.PurchaserTransmitted.Elevator ? "S" : "N",
                            Email = guide.PurchaserTransmitted.Email,
                            Endereco = guide.PurchaserTransmitted.Address,
                            ImovelForeiro = guide.PurchaserTransmitted.PropertyForeiro ? "S" : "N",
                            NomeAdquirente = guide.PurchaserTransmitted.PurchaserName,
                            NomeTransmitente = guide.PurchaserTransmitted.TransmittedName,
                            Numero = guide.PurchaserTransmitted.Number.ToString(),
                            PosicaoPavimento = guide.PurchaserTransmitted.FloorPosition,
                            QtdBanheirosExcetoEmpregada = guide.PurchaserTransmitted.CountBathroomExceptMaid.ToString(),
                            QtdQuartos = guide.PurchaserTransmitted.CountBedrooms.ToString(),
                            QtdVagasEscritura = guide.PurchaserTransmitted.CountParkingSpot.ToString(),
                            QuartoEmpregada = guide.PurchaserTransmitted.BathroomMaid ? "S" : "N",
                            Telefone = guide.PurchaserTransmitted.PhoneNumber,
                            Transmitente = Convert.ToInt32(guide.PurchaserTransmitted.TransmittedOwnerSettings).ToString(),
                            Uf = guide.PurchaserTransmitted.Uf,
                            Varanda = guide.PurchaserTransmitted.Balcony ? "S" : "N"
                        };

                        var requestBuscaAdqCed = new ProcessServiceBase<BuscaAdqCedMapperConfig, BuscaAdqCed>(_configs.BuscaAdqCedMapper, buscaAdqCed, driver);
                        var respBuscaAdqCed = await requestBuscaAdqCed.ExecuteWrite();
                        if (respBuscaAdqCed.Message != "")
                        {
                            await WriteLog(respBuscaAdqCed.Message + "\n" + JsonConvert.SerializeObject(buscaAdqCed));
                            guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Error;
                            guide.Errors = new List<GlobalError>() { new GlobalError() { Code = 0, Field = respBuscaAdqCed.Field, Message = respBuscaAdqCed.Message } };
                            await apiService.PutAsync(guide);
                            continue;
                        }

                        //Loader Pre Protocolo
                        await WriteLog("Carregando Pre Protocolo");
                        var geraPreProtocolo = new GeraPreProtocolo() { };
                        var responseGeraPreProtocolo = new ProcessServiceBase<GeraPreProtocoloMapperConfig, GeraPreProtocolo>(_configs.GeraPreProtocoloMapper, geraPreProtocolo, driver);
                        await responseGeraPreProtocolo.ExecuteRead();

                        guide.PreProtocol = new RoboAntiCaptchaModel.ExternalService.PreProtocol()
                        {
                            Address = geraPreProtocolo.EnderecoImovel,
                            DeclaredValue = Convert.ToDecimal(geraPreProtocolo.ValorDeclarado, cultureInfo),
                            Due = Convert.ToDateTime(geraPreProtocolo.PrazoPagamento, cultureInfo),
                            Iptu = geraPreProtocolo.InscricaoImobiliaria,
                            PercentageTransferred = Convert.ToDecimal(geraPreProtocolo.PorcentagemTransferido.Replace("%", "")),
                            PurchaserDocument = geraPreProtocolo.DocAdquirente,
                            PurchaserInformed = geraPreProtocolo.AdquirenteDigitado,
                            PurchaserVerified = geraPreProtocolo.AdquirenteVerificado,
                            TransactionNature = (TransactionNature)EnumsValues.TransactionNatureDescriptionToValue[geraPreProtocolo.Natureza],
                            TransmittedDocument = geraPreProtocolo.DocTransmitente,
                            TransmittedInformed = geraPreProtocolo.TransmitenteDigitado,
                            TransmittedVerified = geraPreProtocolo.TransmitenteVerificado,
                            ValueItbi = Convert.ToDecimal(geraPreProtocolo.ValorItbi, cultureInfo)
                        };

                        if (guide.Approved)
                        {
                            await responseGeraPreProtocolo.ExecuteWrite();
                            await WriteLog("Carregando Protocolo");
                            var geraProtocolo = new GeraProtocolo() { };
                            var responseGeraProtocolo = new ProcessServiceBase<GeraProtocoloMapperConfig, GeraProtocolo>(_configs.GeraProtocoloMapper, geraProtocolo, driver);
                            await responseGeraProtocolo.ExecuteRead(false);

                            guide.Protocol = new RoboAntiCaptchaModel.ExternalService.Protocol()
                            {
                                Address = geraPreProtocolo.EnderecoImovel,
                                DeclaredValue = Convert.ToDecimal(geraPreProtocolo.ValorDeclarado, cultureInfo),
                                Due = Convert.ToDateTime(geraPreProtocolo.PrazoPagamento, cultureInfo),
                                Iptu = geraPreProtocolo.InscricaoImobiliaria,
                                PercentageTransferred = Convert.ToDecimal(geraPreProtocolo.PorcentagemTransferido.Replace("%", "")),
                                PurchaserDocument = geraPreProtocolo.DocAdquirente,
                                PurchaserInformed = geraPreProtocolo.AdquirenteDigitado,
                                TransactionNature = (TransactionNature)EnumsValues.TransactionNatureDescriptionToValue[geraPreProtocolo.Natureza],
                                TransmittedDocument = geraPreProtocolo.DocTransmitente,
                                TransmittedInformed = geraPreProtocolo.TransmitenteDigitado,
                                ValueItbi = Convert.ToDecimal(geraPreProtocolo.ValorItbi, cultureInfo),
                                ProtocolNumber = Convert.ToInt32(geraProtocolo.Protocolo)
                            };
                            guide.StatusGuide = 1;

                        }

                        guide.Status = Com.ByteAnalysis.IFacilita.Common.Enumerable.APIStatus.Success;

                        await WriteLog("Atualizando informações da Guia de requisição id: " + guide.Id);
                        await apiService.PutAsync(guide);
                        await WriteLog("Guia finalizada com sucesso.");

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

        private async static Task<bool> StartTimer()
        {
            await DisposeTimerAsync();
            await WriteLog("Preparando timer para o ciclo de processamento.");
            _timer = new Timer((1000 * 5) * 1);
            _timer.Elapsed += async (s, e) => { await ExecuteProcess(); };
            _timer.Start();

            return true;
        }

        private async static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            await DisposeTimerAsync();
            await DisposeDriverAsync();
        }

        private static Timer _timerAwait;
        private async static Task<bool> WriteLog(string message)
        {
            try
            {
                if (_timerAwait == null)
                {
                    var myExe = Directory.GetCurrentDirectory() + "/" + "RoboAntiCaptchaConsole.exe";
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
    }
}
