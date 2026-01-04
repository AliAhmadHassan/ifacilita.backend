using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity
{
    public abstract class CertificateBase
    {
        public CertiticateType CertiticateType { get; set; }

        public string DescriptionCertificateType { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
