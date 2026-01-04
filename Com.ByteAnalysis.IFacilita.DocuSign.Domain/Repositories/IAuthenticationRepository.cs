using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories
{
    public interface IAuthenticationRepository : IRepositoryBase<Authentication>
    {
        Task<Authentication> GetByCodeAsync(string code);

        Task<Authentication> GetLastAsync();
    }
}
