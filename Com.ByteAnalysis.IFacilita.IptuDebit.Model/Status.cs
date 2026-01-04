using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.IptuDebit.Model
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
