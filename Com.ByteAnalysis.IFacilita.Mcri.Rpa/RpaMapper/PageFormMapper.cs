using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Attributes;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs
{
    public class PageFormMapper : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string NovoImovel { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NovoImovelEndereco { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string QtdeAdquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string AdquirenteExibir { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string AdquirenteNomeCompleto { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string AdquirenteDocumento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string AdquirenteTipocontribuinte { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string QtdeTransmitente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string TransmitenteExibir { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string TransmitenteNomeCompleto { get; set; }

        //Título de Aquisição
        [MapperConfigAttributes(HtmlElementType = "select")]
        public string Documento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Cartorio { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Oficio { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Livro { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Folha { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Data { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ValorTransacao { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ImpostoTransmicao { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NumeroGuia { get; set; }

        //FRAÇÕES - Consulte a AJUDA
        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string FracaoImovelTipo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string FracaoImovel { get; set; }

        //Dados para Entrega da Guia de IPTU

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NomeEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string DocumentoEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string LogradouroEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Logradouro { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string CodeLogradouroEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NumeroEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ComplementoEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NomeDeclaranteEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string BuscarLogradouroEntrega { get; set; }

        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string Confirmar { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Protocolo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Vencimento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string DataGeracao { get; set; }

    }
}
