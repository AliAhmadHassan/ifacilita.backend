using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Model
{
    public class ShipmentData
    {

        public DateTime TitleDate { get; set; }
        public int Book { get; set; }
        public string Sheet { get; set; }
        public List<PersonalData> Sellers { get; set; }
        public List<PersonalData> Buyers { get; set; }
        public ReceivingNotary ReceivingNotary { get; set; }
        public string Base64Document { get; set; }
    }
}
