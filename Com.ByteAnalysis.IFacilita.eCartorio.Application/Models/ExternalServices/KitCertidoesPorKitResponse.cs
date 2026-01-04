using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.ExternalServices
{
    public class KitCertidoesPorKitResponse
    {
        public int IdKit { get; set; }

        public string Municipio { get; set; }

        public int AtoId { get; set; }

        public string AtoDescricao { get; set; }

        public IEnumerable<int?> Cartorios { get; set; }

        public IEnumerable<decimal?> Valores { get; set; }

        public bool EnvioObrigatorio { get; set; }

        public string TipoAto { get; set; }
    }

    public class CertificateByKitDto
    {
        public int IdKit { get; set; }

        public string City { get; set; }

        public int ActId { get; set; }

        public string ActDescription { get; set; }

        public IEnumerable<int?> Registers { get; set; }

        public IEnumerable<decimal?> Values { get; set; }

        public bool Required { get; set; }

        public string ActType { get; set; }

    }
}
