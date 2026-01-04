using Com.ByteAnalysis.IFacilita.LightOwnership.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Model
{
    public class PhysicalPersonModel
    {
        public string Affiliation01 { get; set; }

        public string Affiliation02 { get; set; }

        public DateTime BirthDate { get; set; }

        public string CodeDocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public string IssuingBody { get; set; }

        public string CodeCountry { get; set; }

        public string CodeUf { get; set; }

        public string Cpf { get; set; }


    }
}
