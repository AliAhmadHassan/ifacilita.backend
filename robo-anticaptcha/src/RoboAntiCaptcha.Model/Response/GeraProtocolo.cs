using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Response
{
    public class GeraProtocolo : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "td")]
        public string Avisos { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string Protocolo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string InscricaoImobiliaria { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DocAdquirenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string AdquirenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DocTransmitenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string TransmitenteDigitado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string PercentualTransferido { get; set; }

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
