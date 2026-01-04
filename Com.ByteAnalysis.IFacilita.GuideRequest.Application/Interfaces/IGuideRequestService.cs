using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Interfaces
{
    public interface IGuideRequestService
    {
        Task<IEnumerable<GuideRequestInput>> GetGuideRequestPendingsAsync();

        Task<IEnumerable<GuideRequestInput>> GetGuideRequestGuidePendingsAsync();

        Task<bool> PutStatusGuidRequestAsync(string guideId, int status);

        Task<GuideRequestInput> GetAsync(string guideId);

        Task<IEnumerable<GuideRequestInput>> GetAsync();

        Task<GuideRequestInput> PostGuideRequestAsync(GuideRequestInput guide);
    }
}
