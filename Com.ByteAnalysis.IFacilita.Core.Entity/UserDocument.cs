using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserDocument: BasicEntity
    {
        public Int32? IdUser { get; set; }
        public string IdentityCard { get; set; }
        public string IdentityCard_FileName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string SocialSecurityNumber_FileName { get; set; }
        public string SpouseIdentityCard { get; set; }
        public string SpouseIdentityCard_FileName { get; set; }
        public string SpouseSocialSecurityNumber { get; set; }
        public string SpouseSocialSecurityNumber_FileName { get; set; }
        public string MarriageCertificate { get; set; }
        public string MarriageCertificate_FileName { get; set; }

    }
}