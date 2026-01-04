using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories
{
    public interface IEnvelopeDocuSignRepository : IRepositoryBase<EnvelopeDocuSign>
    {
        Task<EnvelopeDocuSign> GetByEnvelopeIdAsync(string envelopeId);
    }
}
