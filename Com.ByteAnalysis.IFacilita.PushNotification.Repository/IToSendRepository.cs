using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Repository
{
    public interface IToSendRepository
    {
        Model.ToSend Get(String id);
        IEnumerable<Model.ToSend> Get();

        void Remove(String id);

        Model.ToSend CreateOrUpdate(Model.ToSend toSend);
    }
}
