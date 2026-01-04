using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Request
{
    public class Itbi2ImpGuiasEmitidas : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "captcha")]
        public string Captcha { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Protocol { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Document { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Iptu { get; set; }

    }
}
