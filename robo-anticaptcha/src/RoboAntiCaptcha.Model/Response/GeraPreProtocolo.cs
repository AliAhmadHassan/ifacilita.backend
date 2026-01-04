using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Response
{
    public class GeraPreProtocolo : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "td")]
        public string InscricaoImobiliaria { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DocAdquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string AdquirenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string AdquirenteVerificado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DocTransmitente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string TransmitenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string TransmitenteVerificado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string PorcentagemTransferido { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorItbi { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string PrazoPagamento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string EnderecoImovel { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string Natureza { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorDeclarado { get; set; }

    }
}
