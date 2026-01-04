using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.Impl
{
    public class CertificateMapped
    {
        public CertiticateType CertiticateType { get; set; }

        public string Description { get; set; }
    }

    public class CertificateMapperService
    {
        private readonly List<CertificateMapped> certificates;

        public CertificateMapperService()
        {
            certificates = new List<CertificateMapped>();

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.DefectsDefined,
                Description = "Defeitos Ajuizados"
            });

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.IptuDebts,
                Description = "Débitos do IPTU"
            });

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.RealOnus,
                Description = "Ônus Reais"
            });

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.PropertyRegistrationData,
                Description = "Dados Cadastrais do Imóvel"
            });

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.SearchProtest,
                Description = "Pesquisa de Protestos"
            });

            certificates.Add(new CertificateMapped()
            {
                CertiticateType = CertiticateType.TaxDebts,
                Description = "Débitos Relativos a Créditos Tributários Federais e à dívida ativa da União"
            });
        }

        public CertificateMapped Get(CertiticateType certiticate)
        {
            return certificates.Find(x => x.CertiticateType == certiticate);
        }
    }
}
