using RoboAntiCaptchaModel.Attributes;
using RoboAntiCaptchaModel.MapperConfig;

namespace RoboAntiCaptchaModel.Request
{
    public class BuscaAdqCed : BaseMapperConfig
    {
        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NomeAdquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string Adquirente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Endereco { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Numero { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Complemento { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Bairro { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Cep { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Cidade { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string Uf { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Email { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Ddd { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string Telefone { get; set; }

        [MapperConfigAttributes(HtmlElementType = "text")]
        public string NomeTransmitente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string Transmitente { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string QtdQuartos { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string QtdBanheirosExcetoEmpregada { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string QuartoEmpregada { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string BanheiroEmpregada { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string Elevador { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string QtdVagasEscritura { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string Varanda { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string AreaLazer { get; set; }

        [MapperConfigAttributes(HtmlElementType = "radio")]
        public string ImovelForeiro { get; set; }

        [MapperConfigAttributes(HtmlElementType = "select")]
        public string PosicaoPavimento { get; set; }

    }
}
