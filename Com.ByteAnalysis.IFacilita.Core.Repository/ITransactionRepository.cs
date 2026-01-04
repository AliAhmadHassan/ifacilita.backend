using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface ITransactionRepository: ICrudRepository<Transaction, int>
    {
		List<Transaction> FindByIdUser (Int32 IdUser);
    }
}
