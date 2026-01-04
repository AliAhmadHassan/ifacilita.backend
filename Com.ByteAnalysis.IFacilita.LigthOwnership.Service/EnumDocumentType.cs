using Com.ByteAnalysis.IFacilita.LigthOwnership.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Service
{
    public class EnumDocumentType
    {
        List<EnumParse> list = new List<EnumParse>();
        public EnumDocumentType()
        {
            list = new List<EnumParse>();

            list.Add(new EnumParse { Code = "Z00014", Description = "Agrupamento Poder Público" });
            list.Add(new EnumParse { Code = "Z00005", Description = "Carta de Comprovação de baixa renda" });
            list.Add(new EnumParse { Code = "Z00009", Description = "Carteira de Identidade (RG)" });
            list.Add(new EnumParse { Code = "Z00002", Description = "Carteira de Trabalho" });
            list.Add(new EnumParse { Code = "Z00015", Description = "Carteira Nacional de Habilitação (CNH)" });
            list.Add(new EnumParse { Code = "Z00007", Description = "Entidade Filantropica - COB" });
            list.Add(new EnumParse { Code = "Z00006", Description = "Irregularidades Identificadas" });
            list.Add(new EnumParse { Code = "Z00011", Description = "Número de Identificação do Trabalhador" });
            list.Add(new EnumParse { Code = "Z00010", Description = "Número do Benefício" });
            list.Add(new EnumParse { Code = "Z00004", Description = "Número do NIS" });
            list.Add(new EnumParse { Code = "Z00008", Description = "Outras Empresas - COB" });
            list.Add(new EnumParse { Code = "FS0002", Description = "Passaporte" });
            list.Add(new EnumParse { Code = "Z00012", Description = "Registro de Estrangeiro" });
            list.Add(new EnumParse { Code = "Z00013", Description = "Registro no INCRA" });
            list.Add(new EnumParse { Code = "Z00003", Description = "Registro Profissional (Emitida pelo Conselho de Classe)" });
            list.Add(new EnumParse { Code = "Z00001", Description = "Título de Eleitor" });
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
