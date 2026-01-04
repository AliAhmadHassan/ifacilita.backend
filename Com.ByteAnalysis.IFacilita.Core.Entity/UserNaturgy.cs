using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class UserNaturgy :BasicEntity 
    {
        public Int32 Id { get; set; }
        public bool ChangeTitulary { get; set; }
        public string PasswordNaturgy { get; set; }
        public bool TreatedRobot { get; set; }
        public bool IfacilitaTreats { get; set; }
        public String GuidRobot { get; set; }
        public int IdUser { get; set; }

    }
}
