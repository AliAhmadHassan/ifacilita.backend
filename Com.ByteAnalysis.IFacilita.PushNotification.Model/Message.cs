using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.PushNotification.Model
{
    public class Message
    {
        public Notification Notification { get; set; }
        public string To { get; set; } //Token

    }
}
