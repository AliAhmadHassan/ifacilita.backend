using System;

namespace RoboAntiCaptchaDomain.Entities
{
    public class Address : BaseEntity
    {
        public String Street { get; set; }

        public Int32 Number { get; set; }

        public String Complement { get; set; }

        public String District { get; set; }

        public Int64 ZipCode { get; set; }

        public String CitySig { get; set; }
    }
}
