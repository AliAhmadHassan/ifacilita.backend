using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionDocument: BasicEntity
    {
        public Int32? IdTransaction { get; set; }
        public Int32? IdDocument { get; set; }
    }
}