using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Helpers
{
    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
