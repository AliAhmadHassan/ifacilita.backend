using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IRequirementService
    {
        Task<IEnumerable<RequirementResponse>> GetAsync(string cerp);

        Task<IEnumerable<RequirementResponse>> GetByApplicantAsync(string document);

        Task PostAsync(int idRequiremente, string message);
    }
}
