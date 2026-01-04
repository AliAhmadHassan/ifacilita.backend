using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Data.Repositories
{
    public class AuthenticationRepository : RepositoryBase<Authentication>, IAuthenticationRepository
    {
        public AuthenticationRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("authentication");
        }

        public async Task<Authentication> GetByCodeAsync(string code)
        {
            var result = await base.Collection.Find(x => x.Code.Equals(code)).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Authentication> GetLastAsync()
        {
           var result =  await base.Collection.Find(x => true).SortByDescending(x => x.Created).FirstOrDefaultAsync();
            return result;
        }
    }
}
