using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class PushNotificationReturned
    {
        public long Multicast_id { get; set; }
        public bool Success { get; set; }
        public bool Failure { get; set; }
        public bool CanonicalIds { get; set; }

    }
}
