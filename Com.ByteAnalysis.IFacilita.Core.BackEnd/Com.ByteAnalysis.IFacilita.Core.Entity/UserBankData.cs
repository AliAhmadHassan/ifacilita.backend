using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserBankData: BasicEntity
    {
        public Int32 Id { get; set; }
        public Int32? Agency { get; set; }
        public Int64? Account { get; set; }
        public Int16? AccountDigit { get; set; }
    }
}