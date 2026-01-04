using Com.ByteAnalysis.IFacilita.Chat.Model;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Chat.Service.Impl
{
    public class ConnectionsService : IConnectionsService
    {
        Repository.IConnectionsRepository connectionsRepository;

        public ConnectionsService(Repository.IConnectionsRepository connectionsRepository) => this.connectionsRepository = connectionsRepository;

        public void Add(string uniqueID, User user) => connectionsRepository.Add(uniqueID, user);

        public List<User> GetAllUser() => connectionsRepository.GetAllUser();

        public string GetConnectionId(long id) => connectionsRepository.GetConnectionId(id);

        public string GetUserId(long id) => connectionsRepository.GetUserId(id);
    }
}
