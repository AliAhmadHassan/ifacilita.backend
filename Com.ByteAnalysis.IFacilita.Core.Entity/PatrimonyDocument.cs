using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class PatrimonyDocument: BasicEntity
    {
        public String PatrimonyMunicipalRegistration { get; set; }
        public Int32? IdDocument { get; set; }
        public Patrimony Patrimony { get; set; }
        public Document Document { get; set; }
    }
}