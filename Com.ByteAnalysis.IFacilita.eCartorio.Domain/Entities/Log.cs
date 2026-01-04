using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities
{
    public class Log : EntityBase
    {
        public DateTime Created { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public int HttpStatusCode { get; set; }

    }
}
