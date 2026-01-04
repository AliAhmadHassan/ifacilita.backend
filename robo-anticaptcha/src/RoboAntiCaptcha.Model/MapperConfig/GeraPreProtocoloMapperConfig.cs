using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.Response;

namespace RoboAntiCaptchaModel.MapperConfig
{
    public class GeraPreProtocoloMapperConfig: GeraPreProtocolo
    {
        [MapperConfigAttributes(HtmlElementType = "captcha")]
        public string Captcha { get; set; }

        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string Refazer { get; set; }
    }
}
