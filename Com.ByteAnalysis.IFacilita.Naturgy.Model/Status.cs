using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Naturgy.Model
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
