using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class PlatformBillet: BasicEntity
    {
        public Int64? OurNumber { get; set; }
        public Decimal? Value { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? PayDay { get; set; }
        public Boolean? Paid { get; set; }
        public Int32? IdPlatformBilletBankData { get; set; }
        public Int32? IdUser { get; set; }
    }
}