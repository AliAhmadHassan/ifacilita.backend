using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionRGIService: ICrudService<Entity.TransactionRGI, int>
    {
        void CallRPA(TransactionRGI entity);

        void Completed(int idTransaction);
    }
}
