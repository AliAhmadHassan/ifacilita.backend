using Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Model;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.ExternalServices.Api
{
    public interface IeCartorioApi
    {
        Task<AuthenticationResponse> AuthenticationAsync(AuthenticationRequest auth);

        Task<T> GetAsync<T>(string routeController, NameValueCollection query);

        Task<string> GetStringAsync(string routeController, NameValueCollection query);

        Task<T> PostAsync<T,Y>(string routeController, Y content, NameValueCollection query);

        Task<T> PutAsync<T,Y>(string routeController, Y content, NameValueCollection query);

        Task<T> DeleteAsync<T>(string routeController, NameValueCollection query);
    }
}
