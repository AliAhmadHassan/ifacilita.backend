using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Data.Repositories
{
    public class UserDocuSignRepository : RepositoryBase<User>, IUserDocuSignRepository
    {
        public UserDocuSignRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("");
        }
    }
}
