using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionCertificationRepository: ICrudRepository<Entity.TransactionCertification, int>
    {
        IEnumerable<Entity.TransactionCertification> FindByIdtransaction(int idtransaction);
    }
}
