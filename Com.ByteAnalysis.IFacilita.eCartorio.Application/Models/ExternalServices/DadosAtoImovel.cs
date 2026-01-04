using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class DadosAtoImovel : DadosAto
    {
        public string Matricula { get; set; }

        public string CEP { get; set; }

        public string Municipio { get; set; }

        public string TipoLogradouro { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public List<DadosAtoComplementoImovel> ListaComplementos { get; set; }
    }
}
