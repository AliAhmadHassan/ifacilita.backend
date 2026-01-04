using Com.ByteAnalysis.IFacilita.Mcri.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service.Mapper
{
    public class EnumMapperPropertyFractionType
    {
        List<EnumParse> list = new List<EnumParse>();
        public EnumMapperPropertyFractionType()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse{ Code = 1, Description = "Decimal" });
            list.Add(new EnumParse{ Code = 2, Description = "Percentual" });
            list.Add(new EnumParse { Code = 3, Description = "Fração" });
        }

        public List<EnumParse> GetMapped()
        {
            return list;
        }

        public string GetText(PropertyFractionType enumType)
        {
            return list.FirstOrDefault(x => x.Code == (int)enumType).Description;
        }
    }
}
