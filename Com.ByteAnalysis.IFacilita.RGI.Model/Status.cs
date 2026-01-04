using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Model
{
    public enum Status
    {
        Waiting,
        Processing,
        Finished,
        Delivered,
        ErrorOnCallback
    }
}
