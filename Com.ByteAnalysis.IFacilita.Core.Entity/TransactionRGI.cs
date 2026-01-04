using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionRGI: BasicEntity
    {
        public int IdTransaction { get; set; }
        public DateTime TitleDate { get; set; }
        public int Book { get; set; }
        public string Sheet { get; set; }
        public RegistradoresNotaryCity NotaryCity { get; set; }
        public int NotaryNumber { get; set; }
        public string RpaKey { get; set; }
    }
}
