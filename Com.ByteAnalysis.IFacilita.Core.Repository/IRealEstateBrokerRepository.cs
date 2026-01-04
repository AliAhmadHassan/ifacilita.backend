using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IRealEstateBrokerRepository: ICrudRepository<RealEstateBroker, int>
    {
		List<RealEstateBroker> FindByRealEstateRegisteredNumber (Int64 RealEstateRegisteredNumber);
    }
}
