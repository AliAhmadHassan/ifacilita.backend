using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway;

namespace Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway
{
    public class ClientDto : BaseDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        public string CcEmails { get; set; }

        public string Document { get; set; }

        public string Cep { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string Neighborhood { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Uf { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Ddd { get; set; }

        public string ExternalId { get; set; }
    }
}
