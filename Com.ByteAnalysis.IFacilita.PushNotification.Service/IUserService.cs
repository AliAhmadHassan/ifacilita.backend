using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Service
{
    public interface IUserService
    {
        Model.User CreateOrUpdate(Model.User user);

        Model.User Get(string id);
        Model.User AddToken(string id, string token);
    }
}
