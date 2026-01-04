using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Repositories
{
    public interface IGuideRequestRepository
    {
        Task<List<Entities.GuideRequest>> GetAsync();

        Task<IEnumerable<Entities.GuideRequest>> GetPendingsAsync();

        Task<IEnumerable<Entities.GuideRequest>> GetGuidePendingsAsync();

        Task<Entities.GuideRequest> GetAsync(string id);

        Task<Entities.GuideRequest> CreateAsync(Entities.GuideRequest guideRequest);

        Task UpdateAsync(string id, Entities.GuideRequest guideRequest);

        Task RemoveAsync(Entities.GuideRequest guideRequest);

        Task RemoveAsync(string id);
    }
}
