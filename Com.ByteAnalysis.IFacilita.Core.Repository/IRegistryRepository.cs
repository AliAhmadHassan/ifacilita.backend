using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IRegistryRepository: ICrudRepository<Registry, int>
    {
		List<Registry> FindByIdAddress (Int32 IdAddress);

        IEnumerable<Registry> FindCloser(int idtransaction);

        IEnumerable<Registry> FindByCity(string city);
    }
}
