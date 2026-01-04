using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IPatrimonyRepository: ICrudRepository<Patrimony, int>
    {
		List<Patrimony> FindByIdAddress (Int32 IdAddress);
    }
}
