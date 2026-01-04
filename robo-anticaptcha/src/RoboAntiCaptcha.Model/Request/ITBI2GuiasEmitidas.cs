using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Request
{
    public class ITBI2GuiasEmitidas : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "select")]
        public string ConsultaPor { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NumeroGuiaProtocolo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string CpfCnpjAdquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string InscricaoImobiliaria { get; set; }

        [MapperConfigAttributes(HtmlElementType = "captcha")]
        public string Captcha { get; set; }
    }
}
