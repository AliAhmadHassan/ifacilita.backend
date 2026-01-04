using System;

namespace RoboAntiCaptchaDomain.Entities
{
    public class Patrimony
    {
        public String MunicipalRegistration { get; set; }

        public String Registration { get; set; }

        public Int16 Bedrooms { get; set; }

        public Boolean MaidsRoom { get; set; }

        public Int16 NumberOfCarSpaces { get; set; }

        public Boolean ForeiroProperty { get; set; }

        public Int16 BathroomsExceptForMaids { get; set; }

        public Boolean MaidBathroom { get; set; }

        public Boolean Balcony { get; set; }

        public Int16 FloorPosition { get; set; }

        public Int32 IdAddress { get; set; }

    }
}
