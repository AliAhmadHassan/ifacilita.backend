using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IServiceBase<T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(string id);

        Task<T> CreateAsync(T input);

        Task UpdateAsync(string id, T input);

        Task RemoveAsync(T input);

        Task RemoveAsync(string id);
    }
}
