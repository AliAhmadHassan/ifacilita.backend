using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service
{
    public interface ICertificateService
    {
        List<CertificateEntity> Get();

        CertificateEntity Get(string id);

        CertificateEntity Create(CertificateEntity entry);

        void Update(string id, CertificateEntity entry);

        void Remove(CertificateEntity entry);

        void Remove(string id);
    }
}
