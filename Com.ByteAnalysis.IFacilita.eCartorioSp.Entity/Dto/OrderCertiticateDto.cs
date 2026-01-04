using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto
{
    public class OrderCertiticateDto : OrderDto
    {
        public IEnumerable<CertiticateType> Certificates { get; set; }
    }
}
