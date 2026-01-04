using Com.ByteAnalysis.IFacilita.Chat.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Chat.Repository.Impl
{
    public class UserConected
    {
        public string ConnectionId { get; set; }

        public User User { get; set; }
    }

    public class ConnectionsRepository : IConnectionsRepository
    {
        private readonly static List<UserConected> userConecteds = new List<UserConected>();

        public void Add(string uniqueID, User user)
        {
            var _user = userConecteds.FirstOrDefault(x => x.User.UserId == user.UserId);
            if (_user != null)
                userConecteds.Remove(_user);

            userConecteds.Add(new UserConected()
            {
                ConnectionId = uniqueID,
                User = user
            });

        }

        public string GetUserId(long id)
        {
            return (from con in userConecteds
                    where con.User.UserId == id
                    select con.ConnectionId)
                  .LastOrDefault();
        }

        public List<User> GetAllUser()
        {
            return (from con in userConecteds
                    select con.User)
                    .ToList();
        }

        public string GetConnectionId(long id)
        {
            return (from con in userConecteds
                    where con.User.UserId == id
                    select con.ConnectionId)
                       .LastOrDefault();
        }
    }
}
