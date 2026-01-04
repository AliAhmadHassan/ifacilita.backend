using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public interface IPushNotificationService
    {
        void AddToken(int iduser, string token);

        void SendMessage(int iduser, string title, string body, string icon, string clickAction);
    }
}
