using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Data.Repositories
{
    public class EnvelopeDocuSignRepository : RepositoryBase<EnvelopeDocuSign>, IEnvelopeDocuSignRepository
    {
        public EnvelopeDocuSignRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("envelopeDocuSign");
        }

        public async Task<EnvelopeDocuSign> GetByEnvelopeIdAsync(string envelopeId)
        {
            var result = await base.Collection.Find<EnvelopeDocuSign>(x => x.EnvelopeId == envelopeId).FirstOrDefaultAsync();
            return result;
        }
    }
}
