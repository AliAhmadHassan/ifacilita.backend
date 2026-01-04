using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptcha.Model.Response
{
    public class Simular : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Iptu { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string ValorDeclarado { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NaturezaOperacao { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Pal { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string PercentualTransferido { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string BaseCalculo { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Imposto { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Utilizacao { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Endereco { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Vencimento { get; set; }
    }
}
