using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RoboAntiCaptchaDomain.Interfaces
{
    public interface IConfigMapper
    {
        Task<bool> LoadMappersAsync(IConfiguration configuration);
    }
}
