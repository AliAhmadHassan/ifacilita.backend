using Newtonsoft.Json.Linq;
using Anticaptcha_example.ApiResponse;

namespace Com.ByteAnalysis.IFacilita.OnusReal.RPA
{
    public interface IAnticaptchaTaskProtocol
    {
        JObject GetPostData();
        TaskResultResponse.SolutionData GetTaskSolution();
    }
}
