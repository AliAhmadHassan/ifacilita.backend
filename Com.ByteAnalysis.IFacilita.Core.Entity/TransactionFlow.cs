using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionFlow:BasicEntity
    {
        public int Idtransaction { get; set; }
        public int IdplatformWorkflow { get; set; }
        public int IdplatformSubWorkflow { get; set; }
        public int Status { get; set; }
        public DateTime StatusChanged { get; set; }
    }
}
