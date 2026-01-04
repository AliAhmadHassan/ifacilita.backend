using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities
{
    public class Authentication : EntityBase
    {
        public string AccessToken { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
