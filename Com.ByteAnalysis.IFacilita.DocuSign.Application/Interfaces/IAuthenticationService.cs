using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces
{
    public interface IAuthenticationService : IServiceBase<Authentication>
    {
        Task<Authentication> GetByCodeAsync(string code);

        Task<Authentication> GetLastAsync();
    }
}
