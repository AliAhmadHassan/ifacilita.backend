using System;

namespace RoboAntiCaptchaModel.Attributes
{
    public class MapperConfigAttributes : Attribute
    {
        private string htmlElementType;

        public MapperConfigAttributes()
        {

        }

        public string HtmlElementType { get => htmlElementType; set => htmlElementType = value; }

    }
}
