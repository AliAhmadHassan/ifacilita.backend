using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Models;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Services
{
    public class UserDocuSignService : ServiceBase<User>, IUserDocuSignService
    {
        private readonly IDocuSignClient _docuSignClient;

        public UserDocuSignService(IRepositoryBase<User> repository, IDocuSignClient docuSignClient) : base(repository)
        {
            _repository.SetNameCollection("userDocuSign");
            _docuSignClient = docuSignClient;
        }

        public async Task<UserResponseOutput> GetDocuSignAsync(string id)
        {
            var result = await _docuSignClient.GetUserDocuSignAsync(id);
            return result;
        }

        public async Task<UserOutput> PostDocuSign(UserInput user)
        {
            var result = await _docuSignClient.CreateUserDocuSingAsync(user);

            foreach (var usu in result.NewUsers)
            {
                if (string.IsNullOrEmpty(usu.UserId))
                    continue;

                var userCreated = await _repository.CreateAsync(new User() { Email = usu.Email, UserName = usu.UserName, UserIdDocuSign = usu.UserId });
            }

            return result;
        }
    }
}
