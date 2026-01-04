using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service.Mapper
{
    public class EnumFinancingType
    {
        List<EnumParse> list = new List<EnumParse>();

        public EnumFinancingType()
        {
            
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = "1", Description = "Sistema Financeiro de Habitação" });
            list.Add(new EnumParse { Code = "2", Description = "Minha Casa Minha Vida" });
            list.Add(new EnumParse { Code = "99", Description = "SFI, Carteira Hipotecária, Consórcio, etc" });
        }
        public List<EnumParse> GetMapped()
        {
            return list;
        }

        public string GetText(string code)
        {
            return list.FirstOrDefault(x => x.Code == code).Description;
        }
    }
}
