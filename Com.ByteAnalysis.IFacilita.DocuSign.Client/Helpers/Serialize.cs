using Com.ByteAnalysis.IFacilita.DocuSign.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers
{
    public static class Serialize
    {
        public static string ToJson(this EnvelopeDocuSign self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
