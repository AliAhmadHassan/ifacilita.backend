using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces
{
    public interface IDocuSignClient
    {
        Task<UserOutput> CreateUserDocuSingAsync(UserInput users);

        Task<EnvelopeOutput> CreateEnvelopeDocuSingAsync(EnvelopeInput envelope);

        Task<EnvelopeGetOutput> GetEnvelopeDocuSignAsync(string id);

        Task<UserResponseOutput> GetUserDocuSignAsync(string id);

    }
}
