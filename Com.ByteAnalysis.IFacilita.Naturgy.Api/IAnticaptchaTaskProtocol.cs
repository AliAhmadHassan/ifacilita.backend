using Newtonsoft.Json.Linq;
using Anticaptcha_example.ApiResponse;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Api
{
    public interface IAnticaptchaTaskProtocol
    {
        JObject GetPostData();
        TaskResultResponse.SolutionData GetTaskSolution();
    }
}
