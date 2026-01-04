using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models
{
    public class PropertyDetailsInput
    {
        public string Matricula { get; set; }

        public string CEP { get; set; }

        public string Municipio { get; set; }

        public string TipoLogradouro { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public List<PropertyDetailsComplementInput> ListaComplementos { get; set; }
    }

    public class PropertyDetailsInputDto
    {
        public string Registration { get; set; }

        public string Cep { get; set; }

        public string City { get; set; }

        public string StreetType { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Neighborhood { get; set; }

        public List<PropertyDetailsComplementInput> Complements { get; set; }
    }
}
