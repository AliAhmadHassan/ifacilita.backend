using Com.ByteAnalysis.IFacilita.CertificateESajSp.Model;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Service
{
    public interface ICertificateService
    {
        List<ResumeOrderModel> Get();

        ResumeOrderModel Get(string id);

        ResumeOrderModel Get(string document, DateTime date);

        IEnumerable<ResumeOrderModel> GetPendings();

        ResumeOrderModel Create(ResumeOrderModel entry);

        void Update(string id, ResumeOrderModel entry);

        void Remove(ResumeOrderModel cri);

        void Remove(string id);
    }
}
