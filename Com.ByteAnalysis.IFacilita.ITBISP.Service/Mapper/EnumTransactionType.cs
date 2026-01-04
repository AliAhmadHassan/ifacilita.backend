using Com.ByteAnalysis.IFacilita.ITBISP.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Service.Mapper
{
    public class EnumTransactionType
    {
        List<EnumParse> list = new List<EnumParse>();

        public EnumTransactionType()
        {
            list = new List<EnumParse>();
            list.Add(new EnumParse { Code = "1", Description = "1. Compra e venda" });
            list.Add(new EnumParse { Code = "2", Description = "2. Cessão de direitos relativos a compromisso de compra e venda" });
            list.Add(new EnumParse { Code = "3", Description = "3. Adjudicação (vinculada a processo de execução ou cobrança de dívida)" });
            list.Add(new EnumParse { Code = "4", Description = "4. Arrematação (em leilão ou hasta pública)" });
            list.Add(new EnumParse { Code = "5", Description = "5. Adjudicação compulsória" });
            list.Add(new EnumParse { Code = "6", Description = "6. Cessão de benfeitorias e construções em terreno alheio" });
            list.Add(new EnumParse { Code = "7", Description = "7. Cessão de benfeitorias e construções em terreno compromissado à venda" });
            list.Add(new EnumParse { Code = "8", Description = "8. Cessão de direitos hereditários (ou sucessórios)" });
            list.Add(new EnumParse { Code = "9", Description = "9. Cessão dos direitos do adjudicatário" });
            list.Add(new EnumParse { Code = "10", Description = "10. Cessão dos direitos do arrematante" });
            list.Add(new EnumParse { Code = "11", Description = "11. Cessão de direitos de superfície" });
            list.Add(new EnumParse { Code = "12", Description = "12. Dação em pagamento por escritura pública" });
            list.Add(new EnumParse { Code = "13", Description = "13. Enfiteuse" });
            list.Add(new EnumParse { Code = "14", Description = "14. Mandato em causa própria" });
            list.Add(new EnumParse { Code = "15", Description = "15. Permuta por escritura pública" });
            list.Add(new EnumParse { Code = "16", Description = "16. Remição" });
            list.Add(new EnumParse { Code = "17", Description = "17. Resolução da alienação fiduciária por inadimplemento" });
            list.Add(new EnumParse { Code = "18", Description = "18. Uso" });
            list.Add(new EnumParse { Code = "19", Description = "19. Usufruto" });
            list.Add(new EnumParse { Code = "20", Description = "20. Realização ou integralização de capital " });
            list.Add(new EnumParse { Code = "21", Description = "21. Incorporação" });
            list.Add(new EnumParse { Code = "22", Description = "22. Fusão" });
            list.Add(new EnumParse { Code = "23", Description = "23. Cisão total ou parcial" });
            list.Add(new EnumParse { Code = "24", Description = "24. Extinção de pessoa jurídica" });
            list.Add(new EnumParse { Code = "25", Description = "25. Desincorporação do bem imóvel pertencente à pessoa jurídica" });
            list.Add(new EnumParse { Code = "26", Description = "26. Cessão de direitos sobre o imóvel com alienação fiduciária" });
            list.Add(new EnumParse { Code = "27", Description = "27. Excesso de quinhão ou quota-parte na divisão amigável ou judicial" });
            list.Add(new EnumParse { Code = "28", Description = "28. Excesso de meação ou quinhão na partilha (sucessão causa mortis)" });
            list.Add(new EnumParse { Code = "29", Description = "29. Excesso de meação na partilha (separação / divórcio)" });
            list.Add(new EnumParse { Code = "30", Description = "30. Excesso de meação na partilha (dissolução da união estável)" });
            list.Add(new EnumParse { Code = "31", Description = "31. Rejeição dos embargos à arrematação" });
            list.Add(new EnumParse { Code = "32", Description = "32. Rejeição dos embargos à adjudicação" });
            list.Add(new EnumParse { Code = "33", Description = "33. Demais atos onerosos translativos" });
            list.Add(new EnumParse { Code = "34", Description = "34. Demais sentenças judiciais" });
            list.Add(new EnumParse { Code = "35", Description = "35. Demais transações de direitos de compromisso de compra e venda" });

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
