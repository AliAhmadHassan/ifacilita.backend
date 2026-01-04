using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Address: BasicEntity
    {
        public Int32 Id { get; set; }
        public String Street { get; set; }
        public Int32? Number { get; set; }
        public String Complement { get; set; }
        public String District { get; set; }
        public Int64? ZipCode { get; set; }
        public String CitySig { get; set; }
        public City City { get; set; }
    }
}