using Com.ByteAnalysis.IFacilita.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public interface IUserRepository : ICrudRepository<User, int>
    {
        List<User> FindByIdUserProfile(Int32 IdUserProfile);
        Entity.User FindByEMail(string email);
        Entity.User FindBySocialLoginAuthorizationCode(string authorizationCode);
    }
}
