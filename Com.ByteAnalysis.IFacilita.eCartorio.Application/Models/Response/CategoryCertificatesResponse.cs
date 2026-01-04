namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class CategoryCertificatesResponse
    {
        public string CodigoCartorio { get; set; }

        public string Ato { get; set; }

        public string AtoDescricao { get; set; }

        public int AtoId { get; set; }

        public decimal AtoValor { get; set; }
    }

    public class CertificateByCategoryDto
    {
        public string RegisterCode { get; set; }

        public string Act { get; set; }

        public string ActDescription { get; set; }

        public int ActId { get; set; }

        public decimal ActValue { get; set; }
    }

}
