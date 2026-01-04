using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.Request;

namespace RoboAntiCaptchaModel.MapperConfig
{
    public class EntiGeracaoMapperConfig : EntGeracao
    {
        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string Clear { get; set; }
    }
}
