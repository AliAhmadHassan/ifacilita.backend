using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces
{
    public interface IDocuSignApi<T>
    {
        Task<T> AuthenticationAsync();

        Task<T> AuthenticationCodeAsync();

        Task<bool> GenerateTokenAccess();

        Task<T> PostAsync<Y>(Y obj, params string[] route);

        Task<IEnumerable<T>> GetAllAsync(params string[] route);

        Task<T> GetAsync(params string[] route);

        Task<bool> Delete(params string[] route);

        Task<T> Put(T obj, params string[] route);
    }
}
