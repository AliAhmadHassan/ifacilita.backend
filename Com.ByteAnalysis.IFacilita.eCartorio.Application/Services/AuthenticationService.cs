using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class AuthenticationService : ServiceBase<Authentication>, IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IRepositoryBase<Authentication> repository, IAuthenticationRepository authenticationRepository) : base(repository)
        {
            _repository.SetNameCollection("authentication");
            _authenticationRepository = authenticationRepository;
        }

        public async Task<Authentication> GetLastAsync() => await _authenticationRepository.GetLastAsync();
    }
}
