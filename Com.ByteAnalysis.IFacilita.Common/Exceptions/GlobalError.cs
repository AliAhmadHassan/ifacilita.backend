using System;

namespace Com.ByteAnalysis.IFacilita.Common.Exceptions
{
    public class GlobalError
    {
        public GlobalError()
        {
            DateOccurrence = DateTime.UtcNow;
        }

        public int Code { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
        public DateTime? DateOccurrence { get; set; }
        public string PathImageError { get; set; }
    }
}
