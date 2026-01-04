using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Request
{
    public class EntGeracao : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Adquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Transmitente { get; set; }
    }
}
