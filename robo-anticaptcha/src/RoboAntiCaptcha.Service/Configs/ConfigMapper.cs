using Microsoft.Extensions.Configuration;
using RoboAntiCaptchaDomain.Interfaces;
using RoboAntiCaptchaModel.Config;
using RoboAntiCaptchaModel.MapperConfig;
using System.Threading.Tasks;

namespace RoboAntiCaptchaService.Configs
{
    public class ConfigMapper : IConfigMapper
    {
        public UrlsMapperConfig UrlsMapper { get; set; }
        public AvisoSolicitacaoMapperConfig AvisoSolicitacaoMapper { get; set; }
        public BuscaAdqCedMapperConfig BuscaAdqCedMapper { get; set; }
        public EntSimulacaoMapperConfig EntSimulacaoMapper { get; set; }
        public GeraPreProtocoloMapperConfig GeraPreProtocoloMapper { get; set; }
        public GeraProtocoloMapperConfig GeraProtocoloMapper { get; set; }
        public SimularMapperConfig SimularMapper { get; set; }
        public SolicitacaoGuiaMapperConfig SolicitacaoGuiaMapper { get; set; }
        public EntiGeracaoMapperConfig EntiGeracaoMapper { get; set; }
        public ApiConfigs ApiConfig { get; set; }

        public async Task<bool> LoadMappersAsync(IConfiguration configuration)
        {
            try
            {
                UrlsMapper = configuration.GetSection("Urls").Get<UrlsMapperConfig>();
                AvisoSolicitacaoMapper = configuration.GetSection("AvisoSolicitacao").Get<AvisoSolicitacaoMapperConfig>();
                BuscaAdqCedMapper = configuration.GetSection("BuscaAdqCed").Get<BuscaAdqCedMapperConfig>();
                EntSimulacaoMapper = configuration.GetSection("EntSimulacao").Get<EntSimulacaoMapperConfig>();
                GeraPreProtocoloMapper = configuration.GetSection("GeraPreProtocolo").Get<GeraPreProtocoloMapperConfig>();
                GeraProtocoloMapper = configuration.GetSection("GeraProtocolo").Get<GeraProtocoloMapperConfig>();
                SimularMapper = configuration.GetSection("Simular").Get<SimularMapperConfig>();
                SolicitacaoGuiaMapper = configuration.GetSection("SolicitacaoGuia").Get<SolicitacaoGuiaMapperConfig>();
                EntiGeracaoMapper = configuration.GetSection("EntGeracao").Get<EntiGeracaoMapperConfig>();
                ApiConfig = configuration.GetSection("Api").Get<ApiConfigs>();

                return await Task.FromResult(true);
            }
            catch { return await Task.FromResult(false); }
        }
    }
}
