using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IAddressRepository: ICrudRepository<Address, int>
    {
		List<Address> FindByCitySig (String CitySig);
    }
}
