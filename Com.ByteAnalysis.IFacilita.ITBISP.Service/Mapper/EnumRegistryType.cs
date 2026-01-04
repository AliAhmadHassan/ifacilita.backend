using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service.Mapper
{
    public class EnumRegistryType
    {
        List<EnumParse> list = new List<EnumParse>();

        public EnumRegistryType()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = "1", Description = "1º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "2", Description = "2º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "3", Description = "3º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "4", Description = "4º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "5", Description = "5º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "6", Description = "6º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "7", Description = "7º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "8", Description = "8º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "9", Description = "9º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "10", Description = "10º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "11", Description = "11º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "12", Description = "12º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "13", Description = "13º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "14", Description = "14º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "15", Description = "15º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "16", Description = "16º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "17", Description = "17º Cartório de Registro de Imóvel " });
            list.Add(new EnumParse { Code = "18", Description = "18º Cartório de Registro de Imóvel " });

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
