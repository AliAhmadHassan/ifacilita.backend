using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories
{
    public interface IAuthenticationRepository : IRepositoryBase<Authentication>
    {
        Task<Authentication> GetLastAsync();
    }
}
