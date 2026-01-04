using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.RGI.Model
{
    public class EPropocolo
    {
        public decimal RegistryFees { get; set; }
        public decimal ServiceValues { get; set; }
        public decimal TotalValues { get; set; }
        public string Protocol { get; set; }
        public string ConfirmationScreen { get; set; }
        public string ConfirmationURL { get; set; }
        public string RequestData { get; set; }
    }
}
