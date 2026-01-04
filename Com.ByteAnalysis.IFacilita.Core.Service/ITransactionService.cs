using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ITransactionService: ICrudService<Entity.Transaction, int>
    {
        string MakePromiseDocument(int id);
        bool CallDocusign(int id);
        void BuyerAgreeSignal(Transaction entity);
        void BuyerRecivedSignalValue(Transaction entity);

        bool CallDocusignVFinal(int id);

        string ReciveContract(ViewModel.ContractViewModel contractViewModel);

        Entity.Transaction InformKeyCondition(Entity.Transaction transaction);
    }
}
