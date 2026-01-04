using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.OnusReal.Model
{
    public enum Status
    {
        Waiting,
        Processing,
        ErrorOnProcessing,
        Finished,
        Delivered,
        ErrorOnCallback
    }
}
