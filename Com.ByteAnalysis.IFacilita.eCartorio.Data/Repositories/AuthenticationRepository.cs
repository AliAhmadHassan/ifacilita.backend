using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Data.Repositories
{
    public class AuthenticationRepository : RepositoryBase<Authentication>, IAuthenticationRepository
    {
        public AuthenticationRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("authentication");
        }

        public async Task<Authentication> GetLastAsync()
        {
           var result =  await base.Collection.Find(x => true).SortByDescending(x => x.ExpirationDate).FirstOrDefaultAsync();
            return result;
        }
    }
}
