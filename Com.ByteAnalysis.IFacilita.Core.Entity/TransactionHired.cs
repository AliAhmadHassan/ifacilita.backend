using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionHired : BasicEntity
    {
        public int Id { get; set; }
        public int IdTransaction { get; set; }
        public Transaction Transaction { get; set; }
        public int IdPlatformWorkflow { get; set; }
        public PlatformWorkflow PlatformWorkflow { get; set; }
        public bool Completed { get; set; }
    }
}
