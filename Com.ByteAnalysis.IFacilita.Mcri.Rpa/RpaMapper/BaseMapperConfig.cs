using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Attributes;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs
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
