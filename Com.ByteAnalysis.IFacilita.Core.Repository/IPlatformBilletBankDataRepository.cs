using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IPlatformBilletBankDataRepository: ICrudRepository<PlatformBilletBankData, int>
    {
		List<PlatformBilletBankData> FindByBankId (Int32 BankId);
    }
}
