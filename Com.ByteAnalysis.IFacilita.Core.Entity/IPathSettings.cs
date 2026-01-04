using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class PathSettings : IPathSettings
    {
        public string FilePath { get; set; }
    }
    public interface IPathSettings
    {
        string FilePath { get; set; }
    }
}
