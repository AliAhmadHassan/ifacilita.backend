using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class SendInviteTextType: BasicEntity
    {
        public Int32 Id { get; set; }
        public String Subject { get; set; }
        public String Content { get; set; }
    }
}