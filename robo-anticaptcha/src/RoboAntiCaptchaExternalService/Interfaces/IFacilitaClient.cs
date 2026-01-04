using Com.ByteAnalysis.IFacilita.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoboAntiCaptchaExternalService.Interfaces
{
    public interface IFacilitaClient
    {
        Task<IEnumerable<GuideRequest>> GetPendingAsync();

        Task<IEnumerable<GuideRequest>> GetPendingGuideAsync();

        Task PostAsync(GuideRequest guideRequest);

        Task PutAsync(GuideRequest guideRequest);
    }
}
