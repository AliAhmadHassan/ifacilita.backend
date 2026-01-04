using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class LogService : ServiceBase<Log>, ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(IRepositoryBase<Log> repository, ILogRepository logRepository) : base(repository)
        {
            _repository.SetNameCollection("log");
            _logRepository = logRepository;
        }
    }
}
