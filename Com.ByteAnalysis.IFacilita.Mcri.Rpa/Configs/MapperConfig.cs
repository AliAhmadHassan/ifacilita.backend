using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs
{
    public class MapperConfig
    {
        public ApiConfigs ApiConfig { get; set; }

        public UrlsMapperConfig UrlsMapper { get; set; }
        public PageLoginMapper PageLoginMapper{ get; set; }
        public PageFormMapper  PageFormMapper{ get; set; }

        public async Task<bool> LoadMappersAsync(IConfiguration configuration)
        {
            UrlsMapper = configuration.GetSection("Urls").Get<UrlsMapperConfig>();
            ApiConfig = configuration.GetSection("Api").Get<ApiConfigs>();
            PageLoginMapper = configuration.GetSection("PageLogin").Get<PageLoginMapper>();
            PageFormMapper = configuration.GetSection("PageForm").Get<PageFormMapper>();

            return await Task.FromResult(true);
        }

    }
}
