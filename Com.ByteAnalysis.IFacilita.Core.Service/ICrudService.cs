using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface ICrudService<T, Y> where T: Com.ByteAnalysis.IFacilita.Core.Entity.BasicEntity
    {
        T Insert(T entity);
        T Update(T entity);
        void Delete(Y id);
        T FindById(Y id);
        IEnumerable<T> FindAll();
    }
}
