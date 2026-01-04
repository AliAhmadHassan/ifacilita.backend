using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Impl
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _repository;

        public CertificateService(ICertificateRepository repository)
        {
            _repository = repository;
        }

        public CertificateEntity Create(CertificateEntity entry) => _repository.Create(entry);

        public List<CertificateEntity> Get() => _repository.Get();

        public CertificateEntity Get(string id) => _repository.Get(id);

        public void Remove(CertificateEntity entry) => _repository.Remove(entry);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, CertificateEntity entry)
        {
            _repository.Update(id, entry);
        }
    }
}
