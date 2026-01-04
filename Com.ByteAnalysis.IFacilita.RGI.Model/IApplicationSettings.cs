using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Model
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string EProtocoloUsername { get; set; }
        public string EProtocoloPassword { get; set; }
        public string PresenterDDD { get; set; }
        public string PresenterPhone { get; set; }
    }

    public interface IApplicationSettings
    {
        public string EProtocoloUsername { get; set; }
        public string EProtocoloPassword { get; set; }
        public string PresenterDDD { get; set; }
        public string PresenterPhone { get; set; }
    }
}
