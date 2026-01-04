using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces
{
    public interface IUserDocuSignService : IServiceBase<User>
    {
        Task<UserOutput> PostDocuSign(UserInput user);

        Task<UserResponseOutput> GetDocuSignAsync(string id);

    }
}
