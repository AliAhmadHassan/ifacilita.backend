using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class Sended
    {
        public String IdToSend { get; set; }
        public Message Message { get; set; }
        public DateTime Created { get; set; }
        public DateTime DateOfSended { get; set; }
        public PushNotificationReturned PushNotificationReturned { get; set; }
    }
}
