namespace Com.ByteAnalysis.IFacilita.Mcri.Model
{
    public enum TypeOfContributorType
    {
        SemSelecao = 0,
        PessoaFisica = 21,
        PessoaJuridica = 31,
        UniaoFederalAdministracaoDireta = 41,
        UniaoFederalAutarquiaFederal = 42,
        UniaoFederalFundacaoFederal = 43,
        EstadodoRJAdministracaoDireta = 51,
        EstadodoRJAutarquiaEstadual = 52,
        EstadodoRJFundacaoEstadual = 53,
        MunicipiodoRJAdministracaoDireta = 61,
        MunicipiodoRJAutarquiaMunicipal = 62,
        MunicipiodoRJFundacaoMunicipal = 63
    }

    public enum AcquisitionTitleType
    {
        SemSelecao = 0,
        COMPRAEVENDA,
        CESSAODEDIREITOS,
        PERMUTA,
        DACAOEMPAGAMENTO,
        CARTADEARREMATACAO,
        ADJUDICACAO,
        EXTINAODECONDOMNIO,
        CESSAODEDIREITOSAQUISITIVOS,
        CISAO,
        INCORPORACAO,
        CARTADESENTENA,
        COMPRAEVENDAECESSAO,
        PROMESSADECOMPRAEVENDA,
        PROMESSADECESSAO,
        PROMESSADEPERMUTA,
        AQUISIAODANUAPROPRIEDADE,
        COMPRAEVENDADANUAPROPRIEDADE,
        DOACAO,
        DOACAOCOMRESERVADEUSUFRUTO,
        FORMALDEPARTILHA,
        ALIENACAODOUSUFRUTO,
        EXTINAODOUSUFRUTO,
        RENUNCIADOUSUFRUTO,
        COMPRAEVENDACOMSUBROGACAO,
        USUCAPIAO,
        CANCELAMENTODEINCORPORACAO,
        DISSOLUAODESOCIEDADE,
        CISAOPARCIAL,
        DESAPROPRIACAO,
        INVESTIDURA,
        CONSOLIDACAODEPROPRIEDADE,
        DISTRATO,
        DESINCORPORACAO,
        INTEGRALIZACAODECAPITAL,
        ESCRITURADERERRATIFICACAO,
        ESCRITURADESEPARACAOCONSENSUAL,
        COMPRAEVENDACOMINSTUSUFRUTO,
        ESCRITURADEINSTUIIAODIREITODESUPER,
        INSTITUIAODEFIDEICOMISSO,
        EXTINAODEFIDEICOMISSO,
        COMPRAEVENDACOMINSTUSUFRUTO2,
        ESCRITURAPUBLICADEINVENTRIO,
        INSTITUIAODEUSUFRUTO,
        CONTRATOPARTICULARCOMFORADEESCRITURAPUBLICA,
        RENUNCIACAODIREITODEPROPRIEDADE,
        OUTROS,
    }

    public enum PropertyFractionType
    {
        Decimal,
        Fraction,
        Percent
    }
}
