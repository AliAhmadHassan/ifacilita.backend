using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IActService
    {
        Task<ActResponse> GetAsync(string cerp);

        Task<string> DownloadAsync(string cerp);

        Task<object> ViewAsync(string cerp);

        Task<List<object>> GetRegistryByActAsync(int actId, string city);
    }
}
