using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserSpouse: BasicEntity
    {
        public Int64? SocialSecurityNumber { get; set; }
        public String Name { get; set; }
        public String IdentityCard { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public Int32? MaritalPropertySystems { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
    }
}