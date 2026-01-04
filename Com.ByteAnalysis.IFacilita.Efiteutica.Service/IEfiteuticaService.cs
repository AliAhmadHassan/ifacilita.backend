using Com.ByteAnalysis.IFacilita.Efiteutica.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Service
{
    public interface IEfiteuticaService
    {
        Task<List<RequisitionModel>> GetAsync();

        Task<RequisitionModel> GetAsync(string id);

        Task<IEnumerable<RequisitionModel>> GetPendingsAsync();

        Task<RequisitionModel> CreateAsync(RequisitionModel req);

        Task UpdateAsync(string id, RequisitionModel req);

        Task RemoveAsync(RequisitionModel req);

        Task RemoveAsync(string id);
    }
}
