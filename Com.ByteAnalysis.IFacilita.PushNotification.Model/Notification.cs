using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public string ClickAction { get; set; }
    }
}
