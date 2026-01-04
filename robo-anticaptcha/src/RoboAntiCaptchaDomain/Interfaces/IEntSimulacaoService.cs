using System.Threading.Tasks;

namespace RoboAntiCaptchaDomain.Interfaces
{
    public interface IEntSimulacaoService
    {
        Task<bool> Execute();
    }
}
