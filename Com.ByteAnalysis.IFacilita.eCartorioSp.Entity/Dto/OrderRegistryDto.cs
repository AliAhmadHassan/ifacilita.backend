using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto
{
    public class OrderRegistryDto
    {
        public CertiticateType CertiticateType { get; set; }

        public string Registry { get; set; }

        public IEnumerable<AdditionalInfo> AdditionalInfo { get; set; }
    }

    public class AdditionalInfo
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
