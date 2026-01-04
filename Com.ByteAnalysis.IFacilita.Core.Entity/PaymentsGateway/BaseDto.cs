using System;

namespace Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway
{
    public class BaseDto
    {
        public long Id { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public bool Active { get; set; }
    }
}
