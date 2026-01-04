using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Model
{
    public class RegisterClient
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("socialSecurityNumber")]
        public string CpfCnpj { get; set; }

        [JsonProperty("name")]
        public String FullName { get; set; }

        [JsonProperty("eMail")]
        public String Email { get; set; }

        public string Password { get; set; }

        [JsonProperty("mobileNumber")]
        public String CellPhone { get; set; }

        [JsonProperty("address")]
        public Address? Address { get; set; }

        public string UrlCallBack { get; set; }

        public DateTime Request { get; set; }

        public DateTime CallbackResponse { get; set; }

        public Status StatusProccess { get; set; }

        public DateTime StatusModified { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public APIStatus Status { get; set; }


    }
}
