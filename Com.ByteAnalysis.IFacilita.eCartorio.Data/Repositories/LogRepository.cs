using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Data.Repositories
{
    public class LogRepository : RepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("log");
        }
    }
}
