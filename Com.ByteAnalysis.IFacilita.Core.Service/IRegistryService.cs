using Com.ByteAnalysis.IFacilita.Core.Entity;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IRegistryService: ICrudService<Entity.Registry, int>
    {
        IEnumerable<Registry> FindCloser(int idtransaction);

        IEnumerable<Registry> FindByCity(string city);
    }
}
