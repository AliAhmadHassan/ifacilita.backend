using Com.ByteAnalysis.IFacilita.Mcri.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.Mcri.Service.Mapper
{
    public class EnumMapperAcquisitionTitleType
    {
        List<EnumParse> list = new List<EnumParse>();
        public EnumMapperAcquisitionTitleType()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = 1, Description = "001 - COMPRA E VENDA" });
            list.Add(new EnumParse{ Code = 2, Description = "002 - CESSÃO DE DIREITOS" });
            list.Add(new EnumParse{ Code = 3, Description = "003 - PERMUTA" });
            list.Add(new EnumParse{ Code = 4, Description = "004 - DAÇÃO EM PAGAMENTO" });
            list.Add(new EnumParse{ Code = 5, Description = "005 - CARTA DE ARREMATAÇÃO" });
            list.Add(new EnumParse{ Code = 6, Description = "006 - ADJUDICAÇÃO" });
            list.Add(new EnumParse{ Code = 7, Description = "007 - EXTINÇÃO DE CONDOMÍNIO" });
            list.Add(new EnumParse{ Code = 8, Description = "008 - CESSÃO DE DIREITOS AQUISITIVOS" });
            list.Add(new EnumParse{ Code = 9, Description = "009 - CISÃO" });
            list.Add(new EnumParse{ Code = 10, Description = "010 - INCORPORAÇÃO" });
            list.Add(new EnumParse{ Code = 11, Description = "011 - CARTA DE SENTENÇA" });
            list.Add(new EnumParse{ Code = 12, Description = "012 - COMPRA E VENDA E CESSÃO" });
            list.Add(new EnumParse{ Code = 13, Description = "013 - PROMESSA DE COMPRA E VENDA" });
            list.Add(new EnumParse{ Code = 14, Description = "014 - PROMESSA DE CESSÃO" });
            list.Add(new EnumParse{ Code = 16, Description = "016 - PROMESSA DE PERMUTA" });
            list.Add(new EnumParse{ Code = 17, Description = "017 - AQUISIÇÃO DA NUA PROPRIEDADE" });
            list.Add(new EnumParse{ Code = 18, Description = "018 - COMPRA E VENDA DA NUA PROPRIEDADE" });
            list.Add(new EnumParse{ Code = 19, Description = "019 - DOAÇÃO" });
            list.Add(new EnumParse{ Code = 20, Description = "020 - DOAÇÃO COM RESERVA DE USUFRUTO" });
            list.Add(new EnumParse{ Code = 21, Description = "021 - FORMAL DE PARTILHA" });
            list.Add(new EnumParse{ Code = 22, Description = "022 - ALIENAÇÃO DO USUFRUTO" });
            list.Add(new EnumParse{ Code = 23, Description = "023 - EXTINÇÃO DO USUFRUTO" });
            list.Add(new EnumParse{ Code = 24, Description = "024 - RENÚNCIA DO USUFRUTO" });
            list.Add(new EnumParse{ Code = 26, Description = "026 - COMPRA E VENDA COM SUB-ROGAÇÃO" });
            list.Add(new EnumParse{ Code = 27, Description = "027 - USUCAPIÃO" });
            list.Add(new EnumParse{ Code = 28, Description = "028 - CANCELAMENTO DE INCORPORAÇÃO" });
            list.Add(new EnumParse{ Code = 29, Description = "029 - DISSOLUÇÃO DE SOCIEDADE" });
            list.Add(new EnumParse{ Code = 30, Description = "030 - CISÃO PARCIAL" });
            list.Add(new EnumParse{ Code = 31, Description = "031 - DESAPROPRIAÇÃO" });
            list.Add(new EnumParse{ Code = 32, Description = "032 - INVESTIDURA" });
            list.Add(new EnumParse{ Code = 33, Description = "033 - CONSOLIDAÇÃO DE PROPRIEDADE" });
            list.Add(new EnumParse{ Code = 34, Description = "034 - DISTRATO" });
            list.Add(new EnumParse{ Code = 35, Description = "035 - DESINCORPORAÇÃO" });
            list.Add(new EnumParse{ Code = 36, Description = "036 - INTEGRALIZAÇÃO DE CAPITAL" });
            list.Add(new EnumParse{ Code = 37, Description = "037 - ESCRITURA DE RERRATIFICAÇÃO" });
            list.Add(new EnumParse{ Code = 38, Description = "038 - ESCRITURA DE SEPARAÇÃO CONSENSUAL" });
            list.Add(new EnumParse{ Code = 39, Description = "039 - COMPRA E VENDA COM INST. USUFRUTO" });
            list.Add(new EnumParse{ Code = 40, Description = "040 - ESCRITURA DE INSTUIIÇÃO DIREITO DE SUPER" });
            list.Add(new EnumParse{ Code = 41, Description = "041 - INSTITUIÇÃO DE FIDEICOMISSO" });
            list.Add(new EnumParse{ Code = 42, Description = "042 - EXTINÇÃO DE FIDEICOMISSO" });
            list.Add(new EnumParse{ Code = 43, Description = "043 - COMPRA E VENDA COM INST. USUFRUTO" });
            list.Add(new EnumParse{ Code = 44, Description = "044 - ESCRITURA PÚBLICA DE INVENTÁRIO" });
            list.Add(new EnumParse{ Code = 45, Description = "045 - INSTITUIÇÃO DE USUFRUTO" });
            list.Add(new EnumParse{ Code = 46, Description = "046 - CONTRATO PARTICULAR COM FORÇA DE ESCRITURA PÚBLICA" });
            list.Add(new EnumParse{ Code = 47, Description = "047 - RENUNCIA AO DIREITO DE PROPRIEDADE" });
            list.Add(new EnumParse { Code = 99, Description = "099 - OUTROS" });

        }
        public List<EnumParse> GetMapped()
        {
            return list;
        }

        public string GetText(AcquisitionTitleType enumType)
        {
            return list.FirstOrDefault(x => x.Code == (int)enumType).Description;
        }
    }
}
