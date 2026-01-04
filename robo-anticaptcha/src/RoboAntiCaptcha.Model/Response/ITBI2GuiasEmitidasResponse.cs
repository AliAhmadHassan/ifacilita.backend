using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Response
{
    public class ITBI2GuiasEmitidasResponse : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "td")]
        public string NumeroProtocolo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string NumeroGuia { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string InscricaoImovel { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string CpfCgcAdquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorDeclarado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string BaseCalculo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorImposto { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorMora { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorMulta { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string ValorTotalAPagar { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DataVencimento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "td")]
        public string DisponivelParaPagamento { get; set; }

    }
}
