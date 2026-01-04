namespace Com.ByteAnalysis.IFacilita.GuideRequest.Domain.Entities
{
    public class PurchaserTransmitted
    {
        public string PurchaserName { get; set; }

        public string PurchaserOwnerSettings { get; set; }

        public string Address { get; set; }

        public int Number { get; set; }

        public string Neighborhood { get; set; }

        public string Complement { get; set; }

        public string Cep { get; set; }

        public string City { get; set; }

        public string Uf { get; set; }

        public string Email { get; set; }

        public int Ddd { get; set; }

        public string PhoneNumber { get; set; }

        public string TransmittedName { get; set; }

        public string TransmittedOwnerSettings { get; set; }

        public int CountBedrooms { get; set; }

        public int CountBathroomExceptMaid { get; set; }

        public bool MaidRoom { get; set; }

        public bool BathroomMaid { get; set; }

        public bool Elevator { get; set; }

        public int CountParkingSpot { get; set; }

        public bool Balcony { get; set; }

        public bool RecreationArea { get; set; }

        public bool PropertyForeiro { get; set; }

        public string FloorPosition { get; set; }
    }
}
