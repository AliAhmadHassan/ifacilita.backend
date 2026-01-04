using RoboAntiCaptchaModel.Attributes;

namespace RoboAntiCaptchaModel.MapperConfig
{
    public class AvisoSolicitacaoMapperConfig : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "list")]
        public string Avisos { get; set; }

    }
}
