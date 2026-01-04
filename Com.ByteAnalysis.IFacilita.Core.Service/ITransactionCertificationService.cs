using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionCertificationService: ICrudService<Entity.TransactionCertification, int>
    {
        IEnumerable<Entity.TransactionCertification> FindByIdtransaction(int idtransaction);

        IEnumerable<Entity.TransactionCertification> GetCertificationListForUploadDocument(int idtransaction);

        IEnumerable<Entity.TransactionCertification> GetListCertification(int idTransaction);

        Entity.TransactionCertification RequestIFacilitaRPA(Entity.TransactionCertification entity);

        object MakeECartorioRequest(int idTransaction);

        void UploadCertification(string certificationName, Entity.TransactionCertification entity);
    }
}
