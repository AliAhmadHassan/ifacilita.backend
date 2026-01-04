using Com.ByteAnalysis.IFacilita.Chat.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Chat.Service
{
    public interface IConnectionsService
    {
        void Add(string uniqueID, User user);

        string GetUserId(long id);

        string GetConnectionId(long id);

        List<User> GetAllUser();
    }
}
