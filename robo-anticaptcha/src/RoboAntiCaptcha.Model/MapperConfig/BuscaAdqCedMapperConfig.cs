using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.Request;

namespace RoboAntiCaptchaModel.MapperConfig
{
    public class BuscaAdqCedMapperConfig: BuscaAdqCed
    {
        [MapperConfigAttributes(HtmlElementType = "buttom")]
        public string Desfazer { get; set; }
    }
}
