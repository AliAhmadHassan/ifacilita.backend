using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> List(string city);

        Task<IEnumerable<CategoryCertificatesResponse>> CertificatesByCategory(int idCategory, string city);

    }
}
