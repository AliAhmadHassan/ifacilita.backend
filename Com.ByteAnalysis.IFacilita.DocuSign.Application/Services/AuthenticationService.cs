using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Services
{
    public class AuthenticationService : ServiceBase<Authentication>, IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        public AuthenticationService(IRepositoryBase<Authentication> repository, IAuthenticationRepository authenticationRepository) : base(repository)
        {
            _repository.SetNameCollection("authentication");
            _authenticationRepository = authenticationRepository;
        }

        public async Task<Authentication> GetByCodeAsync(string code) => await _authenticationRepository.GetByCodeAsync(code);

        public async Task<Authentication> GetLastAsync() => await _authenticationRepository.GetLastAsync();
    }
}
