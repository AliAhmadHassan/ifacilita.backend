using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Model
{
    public class Address
    {
        [JsonProperty("zipCode")]
        public int Cep { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

    }
}
