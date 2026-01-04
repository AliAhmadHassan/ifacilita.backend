using System;

namespace Com.ByteAnalysis.IFacilita.Draft.Model
{
    public class Seller
    {
        public string Name { get; set; }
        public Int64 SocialSecurityNumber { get; set; }
        public String IdentityCard { get; set; }
        public string Nationality { get; set; }
        public string MaritalPropertySystems { get; set; }
        public string SpouseName { get; set; }
        public string ExpeditionLocal { get; set; }
        public DateTime ExpeditionDate { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public Int32 IdUserSpouseType { get; set; }
    }
}
