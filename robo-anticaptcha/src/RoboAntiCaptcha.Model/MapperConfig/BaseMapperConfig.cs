using RoboAntiCaptchaModel.Attributes;

namespace RoboAntiCaptchaModel.MapperConfig
{
    public abstract class BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "pagename")]
        public string PageName { get; set; }

        [MapperConfigAttributes(HtmlElementType = "textid")]
        public string TextId { get; set; }

        [MapperConfigAttributes(HtmlElementType = "submit")]
        public string Submeter { get; set; }
    }
}
