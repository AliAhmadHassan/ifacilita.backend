namespace RoboAntiCaptchaModel.Enums
{
    public enum TransactionNature
    {
        Compra_e_Venda = 1,
        Aquisicao_da_Nua_Propriedade = 10,
        Cessao_de_Dir_Aquisitivos_Decorrentes_de_Promessa = 21,
        Cessao_de_Direitos_Hereditarios = 7,
        Dacao_em_Pagamento = 9,
        Inst_Usufruto_Uso_e_Habitacao = 11,
        Permuta = 8,
        Outorga_Dir_Real_Superficie = 36,
        Integraliz_Real_Cap_Pgto_ITBI_ANTES_Reg_Junta = 87,
        Consolidacao_de_Propriedade = 99
    }

    public static class EnumsValues
    {
        public static System.Collections.Generic.Dictionary<string, int> TransactionNatureDescriptionToValue
            = new System.Collections.Generic.Dictionary<string, int>()
            {
                {"Compra e Venda", 1 },
                {"COMPRA E VENDA", 1 },
                {"Aquisição da Nua Propriedade", 10 },
                {"AQUISIÇÃO DA NUA PROPRIEDADE", 10 },
                {"Cessão de Dir. Aquisitivos Decorrentes de Promessa", 21 },
                {"CESSÃO DE DIR. AQUISITIVOS DECORRENTES DE PROMESSA", 21 },
                {"Cessão de Direitos Hereditários", 7 },
                {"CESSÃO DE DIREITOS HEREDITÁRIOS", 7 },
                {"Dação em Pagamento", 9 },
                {"DAÇÃO EM PAGAMENTO", 9 },
                {"Inst. Usufruto, Uso e Habitação", 11 },
                {"INST. USUFRUTO, USO E HABITAÇÃO", 11 },
                {"Permuta", 8 },
                {"PERMUTA", 8 },
                {"Outorga Dir Real Superfície", 36 },
                {"OUTORGA DIR REAL SUPERFÍCIE", 36 },
                {"Integraliz. Real.Cap. - Pgto ITBI ANTES Reg Junta", 87 },
                {"INTEGRALIZ. REAL.CAP. - PGTO ITBI ANTES REG JUNTA", 87 },
                {"Consolidação de Propriedade", 99 },
                {"CONSOLICAÇÃO DE PROPRIEDADE", 99 },
            };
    }


}
