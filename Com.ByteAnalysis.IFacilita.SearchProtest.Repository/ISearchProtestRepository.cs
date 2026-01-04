using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Repository
{
    public interface ISearchProtestRepository
    {
         List<RequestSearchProtestModel> Get();

        RequestSearchProtestModel Get(string id);

        IEnumerable<RequestSearchProtestModel> GetPendings();

        RequestSearchProtestModel Create(RequestSearchProtestModel entity);

        void Update(string id, RequestSearchProtestModel entity);

        void Remove(RequestSearchProtestModel entity);

        void Remove(string id);
    }
}
