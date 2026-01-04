using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Transaction: BasicEntity
    {
        public Int32 Id { get; set; }
        public DateTime? Date { get; set; }
        public Int32? IdUser { get; set; }
        public Int32? Seller { get; set; }
        public String? PatrimonyMunicipalRegistration { get; set; }
        public Int32? IdPatrimonyAcquirerType { get; set; }
        public Int32? IdPatrimonyTransmitterType { get; set; }
        public Int32? IdRegistry { get; set; }
    }
}