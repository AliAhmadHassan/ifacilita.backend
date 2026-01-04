using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IHistoricService: ICrudService<Entity.Historic, int>
    {
        IEnumerable<Entity.Historic> FindByIdTransaction(int idTransaction);
    }
}
