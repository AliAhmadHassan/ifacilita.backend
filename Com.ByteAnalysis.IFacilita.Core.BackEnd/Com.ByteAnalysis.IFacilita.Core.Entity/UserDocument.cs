using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserDocument: BasicEntity
    {
        public Int32? IdUser { get; set; }
        public Int32? IdDocument { get; set; }
    }
}