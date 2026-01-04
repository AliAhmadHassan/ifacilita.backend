using Com.ByteAnalysis.IFacilita.Mcri.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service
{
    public interface IMcriService
    {

        List<CriModel> Get();

        CriModel Get(string id);

        IEnumerable<CriModel> GetPendings();

        CriModel Create(CriModel cri);

        void Update(string id, CriModel cri);

        void Remove(CriModel cri);

        void Remove(string id);
    }
}
