using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Repository
{
    public interface IRepository
    {
        List<OwnershipModel> Get();

        OwnershipModel Get(string id);

        IEnumerable<OwnershipModel> GetPendings();

        OwnershipModel Create(OwnershipModel owner);

        void Update(string id, OwnershipModel owner);

        void Remove(OwnershipModel owner);

        void Remove(string id);
    }
}
