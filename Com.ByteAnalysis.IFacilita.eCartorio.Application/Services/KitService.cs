using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces.ExternalServices;
using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Services
{
    public class KitService : IKitService
    {
        private readonly IeCartorioClient _ieCartorioClient;

        public KitService(IeCartorioClient ieCartorioClient, ILogService logService)
        {
            _ieCartorioClient = ieCartorioClient;
        }

        public async Task<IEnumerable<KitCertidoesPorKitResponse>> GetCertificatesByKitAsync(int idKit, string municipio)
        {
            var result = await _ieCartorioClient.KitListarCertidoesPorKitAsync(idKit, municipio);
            return result;
        }

        public async Task<IEnumerable<KitListarResponse>> ListAsync(string municipio)
        {
            return await _ieCartorioClient.KitListarAsync(municipio);
        }
    }
}
