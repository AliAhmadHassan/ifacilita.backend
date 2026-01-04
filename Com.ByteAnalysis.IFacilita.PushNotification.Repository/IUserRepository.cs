using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Repository
{
    public interface IUserRepository
    {
        Model.User CreateOrUpdate(Model.User user);

        public Model.User Get(string id);
    }
}
