using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IUserService : ICrudService<Entity.User, int>
    {
        Entity.User InsertWithSocialLogin(Entity.User user);
        Entity.User FindBySocialLoginAuthorizationCode(string authorizationCode);
    }
}
