using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IAuthenticationService : IServiceBase<Authentication>
    {
        Task<Authentication> GetLastAsync();
    }
}
