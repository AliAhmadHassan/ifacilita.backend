using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class PatrimonyIptu : BasicEntity
    {
        public int Id { get; set; }
        public int IdDocument { get; set; }
        public int RegistryNumber { get; set; }
        public int Craft { get; set; }
        public int Book { get; set; }
        public int Paper { get; set; }
        public DateTime Date { get; set; }
        public int GuideNumber { get; set; }
        public DateTime Created { get; set; }
        public string PatrimonyMunicipalRegistration { get; set; }
        public Patrimony Patrimony { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal TaxValue { get; set; }
    }
}
