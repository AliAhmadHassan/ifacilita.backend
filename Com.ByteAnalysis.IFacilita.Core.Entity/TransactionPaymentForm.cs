using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionPaymentForm: BasicEntity
    {
        public int Id { get; set; }
        public int IdTransaction { get; set; }
        public int? Plain { get; set; }
        public decimal? Value { get; set; }
    }
}
