using Com.ByteAnalysis.IFacilita.SearchProtest.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.SearchProtest.Service
{
    public interface ISearchProtestService
    {
        List<RequestSearchProtestModel> Get();

        RequestSearchProtestModel Get(string id);

        IEnumerable<RequestSearchProtestModel> GetPendings();

        RequestSearchProtestModel Create(RequestSearchProtestModel entry);

        void Update(string id, RequestSearchProtestModel entry);

        void Remove(RequestSearchProtestModel entry);

        void Remove(string id);
    }
}
