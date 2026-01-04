using Com.ByteAnalysis.IFacilita.Mcri.Rpa.Attributes;

namespace Com.ByteAnalysis.IFacilita.Mcri.Rpa.Configs
{
    public class PageLoginMapper : BaseMapperConfig
    {
        private string iptu;
        private string captcha;

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Iptu { get => iptu; set => iptu = value; }

        [MapperConfigAttributes(HtmlElementType = "captcha2")]
        public string Captcha { get => captcha; set => captcha = value; }
    }
}
