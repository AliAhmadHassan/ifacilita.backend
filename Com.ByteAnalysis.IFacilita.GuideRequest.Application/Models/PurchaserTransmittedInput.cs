using Com.ByteAnalysis.IFacilita.GuideRequest.Application.Enums;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models
{
    public class PurchaserTransmittedInput
    {
        public string PurchaserName { get; set; }

        public OwnerSettings PurchaserOwnerSettings { get; set; }

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

        public OwnerSettings TransmittedOwnerSettings { get; set; }

        public int CountBedrooms { get; set; }

        public int CountBathroomExceptMaid { get; set; }

        public bool MaidRoom { get; set; }

        public bool BathroomMaid { get; set; }

        public bool Elevator { get; set; }

        public int CountParkingSpot { get; set; }

        public bool Balcony { get; set; }

        public bool RecreationArea { get; set; }

        /// <summary>
        /// Imóvel Foreiro
        /// </summary>
        public bool PropertyForeiro { get; set; }

        /// <summary>
        /// Posição do Pavimento
        /// </summary>
        /// <example>Terreo, 01 andar, 02 andar, 03 andar ...</example>
        public string FloorPosition { get; set; }

    }
}
