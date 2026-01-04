using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IKitService
    {
        Task<IEnumerable<KitListarResponse>> ListAsync(string municipio);

        Task<IEnumerable<KitCertidoesPorKitResponse>> GetCertificatesByKitAsync(int idKit, string municipio);
    }
}
