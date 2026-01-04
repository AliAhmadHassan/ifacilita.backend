using System;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Registry: BasicEntity
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public Int32? IdAddress { get; set; }
        public Address Address { get; set; }
        public string City { get; set; }
    }
}