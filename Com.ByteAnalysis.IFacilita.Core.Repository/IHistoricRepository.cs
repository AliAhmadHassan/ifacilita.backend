using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IHistoricRepository: ICrudRepository<Entity.Historic, int>
    {
        IEnumerable<Entity.Historic> FindByIdTransaction(int idTransaction);
    }
}
