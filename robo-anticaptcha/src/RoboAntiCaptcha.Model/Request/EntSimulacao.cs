using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Request
{
    public class EntSimulacao : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Iptu { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ValorDeclarado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string NaturezaOperacao { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Pal { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ParteTransferida { get; set; }

        [MapperConfigAttributes(HtmlElementType = "captcha")]
        public string Captcha { get; set; }

    }
}
