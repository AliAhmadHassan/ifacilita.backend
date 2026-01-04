using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Registry: BasicEntity
    {
        public Int32 Id { get; set; }
        public String? Name { get; set; }
        public Int32? IdAddress { get; set; }
    }
}