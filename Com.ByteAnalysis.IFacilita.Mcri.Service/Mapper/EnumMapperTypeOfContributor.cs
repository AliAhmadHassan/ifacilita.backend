using Com.ByteAnalysis.IFacilita.Mcri.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service.Mapper
{
    public class EnumMapperTypeOfContributor
    {
        List<EnumParse> list = new List<EnumParse>();

        public EnumMapperTypeOfContributor()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = 21, Description = "21 - Pessoa Física" });
            list.Add(new EnumParse { Code = 31, Description = "31 - Pessoa Jurídica" });
            list.Add(new EnumParse { Code = 41, Description = "41 - União Federal – Administração Direta" });
            list.Add(new EnumParse { Code = 42, Description = "42 - União Federal – Autarquia Federal" });
            list.Add(new EnumParse { Code = 43, Description = "43 - União Federal – Fundação Federal" });
            list.Add(new EnumParse { Code = 51, Description = "51 - Estado do RJ – Administração Direta" });
            list.Add(new EnumParse { Code = 52, Description = "52 - Estado do RJ – Autarquia Estadual" });
            list.Add(new EnumParse { Code = 53, Description = "53 - Estado do RJ – Fundação Estadual" });
            list.Add(new EnumParse { Code = 61, Description = "61 - Município do RJ – Administração Direta" });
            list.Add(new EnumParse { Code = 62, Description = "62 - Município do RJ – Autarquia Municipal" });
            list.Add(new EnumParse { Code = 63, Description = "63 - Município do RJ – Fundação Municipal" });
        }

        public List<EnumParse> GetMapped()
        {
            return list;
        }

        public string GetText(TypeOfContributorType typeOfContributorType)
        {
            return list.FirstOrDefault(x => x.Code == (int)typeOfContributorType).Description;
        }
    }
}
