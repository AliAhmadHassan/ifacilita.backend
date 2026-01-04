using Com.ByteAnalysis.IFacilita.Efiteutica.Model;
using Com.ByteAnalysis.IFacilita.Efiteutica.Repository;
using Com.ByteAnalysis.IFacilita.Efiteutica.Repository.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Efiteutica.Service.Impl
{
    public class EfiteuticaService : IEfiteuticaService
    {
        private readonly IEfiteuticaRepository _efiteuticaRepository;
        public EfiteuticaService(IEfiteuticaRepository efiteuticaRepository)
        {
            _efiteuticaRepository = efiteuticaRepository;
        }

        public async Task<RequisitionModel> CreateAsync(RequisitionModel req) 
            => await _efiteuticaRepository.CreateAsync(req);

        public async Task<List<RequisitionModel>> GetAsync()
            => await _efiteuticaRepository.GetAsync();

        public async Task<RequisitionModel> GetAsync(string id)
            => await _efiteuticaRepository.GetAsync(id);

        public async Task<IEnumerable<RequisitionModel>> GetPendingsAsync()
            => await _efiteuticaRepository.GetPendingsAsync();

        public async Task RemoveAsync(RequisitionModel req)
            => await _efiteuticaRepository.RemoveAsync(req);

        public async Task RemoveAsync(string id)
            => await _efiteuticaRepository.RemoveAsync(id);

        public async Task UpdateAsync(string id, RequisitionModel req)
            => await _efiteuticaRepository.UpdateAsync(id, req);
    }
}
