using Com.ByteAnalysis.IFacilita.LigthOwnership.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.LigthOwnership.Service
{
    public class EnumUfType
    {
        List<EnumParse> list = new List<EnumParse>();
        public EnumUfType()
        {
            list = new List<EnumParse>();

            list.Add(new EnumParse { Code = "AC", Description = "AC" });
            list.Add(new EnumParse { Code = "AL", Description = "AL" });
            list.Add(new EnumParse { Code = "AP", Description = "AP" });
            list.Add(new EnumParse { Code = "AM", Description = "AM" });
            list.Add(new EnumParse { Code = "BA", Description = "BA" });
            list.Add(new EnumParse { Code = "CE", Description = "CE" });
            list.Add(new EnumParse { Code = "DF", Description = "DF" });
            list.Add(new EnumParse { Code = "ES", Description = "ES" });
            list.Add(new EnumParse { Code = "GO", Description = "GO" });
            list.Add(new EnumParse { Code = "MA", Description = "MA" });
            list.Add(new EnumParse { Code = "MS", Description = "MS" });
            list.Add(new EnumParse { Code = "MT", Description = "MT" });
            list.Add(new EnumParse { Code = "MG", Description = "MG" });
            list.Add(new EnumParse { Code = "PA", Description = "PA" });
            list.Add(new EnumParse { Code = "PB", Description = "PB" });
            list.Add(new EnumParse { Code = "PR", Description = "PR" });
            list.Add(new EnumParse { Code = "PE", Description = "PE" });
            list.Add(new EnumParse { Code = "PI", Description = "PI" });
            list.Add(new EnumParse { Code = "RJ", Description = "RJ" });
            list.Add(new EnumParse { Code = "RN", Description = "RN" });
            list.Add(new EnumParse { Code = "RS", Description = "RS" });
            list.Add(new EnumParse { Code = "RO", Description = "RO" });
            list.Add(new EnumParse { Code = "RR", Description = "RR" });
            list.Add(new EnumParse { Code = "SC", Description = "SC" });
            list.Add(new EnumParse { Code = "SP", Description = "SP" });
            list.Add(new EnumParse { Code = "SE", Description = "SE" });
            list.Add(new EnumParse { Code = "TO", Description = "TO" });
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
