using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Repository
{
    public interface ICertificateRepository
    {
        List<CertificateEntity> Get();

        CertificateEntity Get(string id);

        CertificateEntity Create(CertificateEntity entity);

        void Update(string id, CertificateEntity entity);

        void Remove(CertificateEntity entity);

        void Remove(string id);
    }
}
