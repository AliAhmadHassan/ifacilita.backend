using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Service
{
    public interface IToSendService
    {
        Model.ToSend Get(String Id);
        IEnumerable<Model.ToSend> Get();

        void Remove(String id);

        void CreateOrUpdate(string iduser, Model.Notification notification);

        bool Send();
        bool Send(Model.ToSend toSend);
    }
}
