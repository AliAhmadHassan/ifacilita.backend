using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway
{
    public interface IClientApi
    {
        Task<T> GetAsync<T>(string routeController, string token, NameValueCollection query);

        Task<T> PostAsync<T, Y>(string routeController, Y content, string token, NameValueCollection query);

        Task<T> PutAsync<T, Y>(string routeController, Y content, string token, NameValueCollection query);

        Task<T> DeleteAsync<T>(string routeController, string token, NameValueCollection query);
    }
}
