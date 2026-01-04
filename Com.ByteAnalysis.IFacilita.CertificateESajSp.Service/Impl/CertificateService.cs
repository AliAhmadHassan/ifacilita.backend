using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using Com.ByteAnalysis.IFacilita.CertificateESajSp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Service.Impl
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _repository;

        public CertificateService(ICertificateRepository repository)
        {
            _repository = repository;
        }

        public ResumeOrderModel Create(ResumeOrderModel cri) => _repository.Create(cri);

        public List<ResumeOrderModel> Get() => _repository.Get();

        public ResumeOrderModel Get(string id) => _repository.Get(id);

        public ResumeOrderModel Get(string document, DateTime date)
            => _repository.GetByDocument(document).Where(x => x.DataOrder != null && x.DataOrder.DateOrder.Date.Equals(date)).FirstOrDefault();

        public IEnumerable<ResumeOrderModel> GetPendings() => _repository.GetPendings();

        public void Remove(ResumeOrderModel cri) => _repository.Remove(cri);

        public void Remove(string id) => _repository.Remove(id);

        public void Update(string id, ResumeOrderModel cri) => _repository.Update(id, cri);
    }
}
