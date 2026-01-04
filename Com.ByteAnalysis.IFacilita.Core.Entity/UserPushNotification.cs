using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserPushNotification
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Token { get; set; }
    }
}
