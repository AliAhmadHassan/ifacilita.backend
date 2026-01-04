using Microsoft.Extensions.Configuration;
using RoboAntiCaptchaDomain.Interfaces;
using RoboAntiCaptchaModel.Config;
using RoboAntiCaptchaModel.MapperConfig;
using System.Threading.Tasks;

namespace RoboAntiCaptchaService.Configs
{
    public class ConfigBilletMapper : IConfigMapper
    {
        public UrlsMapperConfig UrlsMapper { get; set; }

        public ApiConfigs ApiConfig { get; set; }

        public ITBI2GuiasEmitidasMapperConfig iTBI2Config { get; set; }

        public ITBI2GuiasEmitidasResponseMapperConfig iTBI2ResponseConfig { get; set; }

        public Itbi2ImpGuiasEmitidasMapperConfig Itbi2ImpGuiasEmitidasConfig { get; set; }

        public async Task<bool> LoadMappersAsync(IConfiguration configuration)
        {
            UrlsMapper = configuration.GetSection("Urls").Get<UrlsMapperConfig>();
            ApiConfig = configuration.GetSection("Api").Get<ApiConfigs>();
            iTBI2Config = configuration.GetSection("ITBI2GuiasEmitidas").Get<ITBI2GuiasEmitidasMapperConfig>();
            iTBI2ResponseConfig = configuration.GetSection("ITBI2GuiasEmitidasResponse").Get<ITBI2GuiasEmitidasResponseMapperConfig>();
            Itbi2ImpGuiasEmitidasConfig = configuration.GetSection("Itbi2ImpGuiasEmitidas").Get<Itbi2ImpGuiasEmitidasMapperConfig>();

            return await Task.FromResult(true);
        }
    }
}
