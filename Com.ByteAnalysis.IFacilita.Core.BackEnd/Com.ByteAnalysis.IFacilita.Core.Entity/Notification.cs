using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Notification: BasicEntity
    {
        public Int32 Id { get; set; }
        public DateTime? Created { get; set; }
        public String? Message { get; set; }
        public Boolean? Readed { get; set; }
        public DateTime? WhenReaded { get; set; }
        public Int32? IdUser { get; set; }
    }
}