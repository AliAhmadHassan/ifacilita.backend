using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Repository
{
    public interface ICertificateRepository
    {
        List<ResumeOrderModel> Get();

        ResumeOrderModel Get(string id);

        IEnumerable<ResumeOrderModel> GetByDocument(string document);

        IEnumerable<ResumeOrderModel> GetPendings();

        ResumeOrderModel Create(ResumeOrderModel entry);

        void Update(string id, ResumeOrderModel entry);

        void Remove(ResumeOrderModel entry);

        void Remove(string id);
    }
}
