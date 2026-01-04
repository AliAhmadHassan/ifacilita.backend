using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string URLPushNotification { get; set; }
        public string URLRGIApi { get; set; }

    }

    public interface IApplicationSettings
    {
        string URLPushNotification { get; set; }
        string URLRGIApi { get; set; }
    }
}
