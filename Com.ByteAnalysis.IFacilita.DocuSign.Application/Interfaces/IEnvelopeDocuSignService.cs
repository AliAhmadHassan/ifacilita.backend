using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces
{
    public interface IEnvelopeDocuSignService : IServiceBase<EnvelopeDocuSign>
    {
        Task<EnvelopeOutput> PostDocuSign(EnvelopeInput envelope);

        Task<EnvelopeGetOutput> GetDocuSignAsync(string id);

        Task<EnvelopeDocuSign> GetByEnvelopeIdAsync(string envelopeId);
    }
}
