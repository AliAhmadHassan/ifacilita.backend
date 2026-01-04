using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> GetAsync(string id);

        Task<T> CreateAsync(T guideRequest);

        Task UpdateAsync(string id, T guideRequest);

        Task RemoveAsync(T guideRequest);

        Task RemoveAsync(string id);

        void SetNameCollection(string nameCollection);
    }
}
