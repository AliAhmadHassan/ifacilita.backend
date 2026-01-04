using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public interface IApplicationSettings
    {
        public string PushNotificationToken { get; set; }
    }

    public class ApplicationSettings : IApplicationSettings
    {
        public string PushNotificationToken { get; set; }
    }
}
