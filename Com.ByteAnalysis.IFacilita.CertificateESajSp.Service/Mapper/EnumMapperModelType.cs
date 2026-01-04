using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.CertificateESajSp.Service.Mapper
{
    public class EnumMapperModelType
    {
        List<EnumParse> list = new List<EnumParse>();
        public EnumMapperModelType()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = 58, Description = "CERT DIST - FALÊNCIAS, CONCORDATAS E RECUPERAÇÕES" });
            list.Add(new EnumParse { Code = 54, Description = "CERT DIST - INVENTÁRIOS, ARROLAMENTOS E TESTAMENTOS" });
            list.Add(new EnumParse { Code = 94, Description = "CERTIDÃO  DE EXECUÇÃO CRIMINAL - SAJ PG5" });
            list.Add(new EnumParse { Code = 3, Description = "CERTIDÃO CRIMINAL PARA FINS ELEITORAIS" });
            list.Add(new EnumParse { Code = 41, Description = "CERTIDÃO DE DISTRIBUIÇÃO CÍVEL EM GERAL - ATÉ 10 ANOS" });
            list.Add(new EnumParse { Code = 52, Description = "CERTIDÃO DE DISTRIBUIÇÃO CÍVEL EM GERAL - MAIS DE 10 ANOS" });
            list.Add(new EnumParse { Code = 6, Description = "CERTIDÃO DE DISTRIBUIÇÃO DE AÇÕES CRIMINAIS" });
            list.Add(new EnumParse { Code = 95, Description = "CERTIDÃO DE EXECUÇÃO CRIMINAL - SIVEC" });
            list.Add(new EnumParse { Code = 97, Description = "CERTIDÃO DE EXECUÇÕES CRIMINAIS FINS ELEITORAIS - SAJ PG5" });
            list.Add(new EnumParse { Code = 45, Description = "CERTIDÃO DE EXECUÇÕES CRIMINAIS PARA FINS ELEITORAIS - SIVEC" });
        }

        public List<EnumParse> GetMapped()
        {
            return list;
        }

        public string GetText(int code)
        {
            return list.FirstOrDefault(x => x.Code == code).Description;
        }
    }
}
