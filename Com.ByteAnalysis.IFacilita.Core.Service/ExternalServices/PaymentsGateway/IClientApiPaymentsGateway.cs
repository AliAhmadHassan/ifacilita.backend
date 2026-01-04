using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway;
using Com.ByteAnalysis.IFacilita.Core.Entity.PaymentsGateway;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.Core.Service.ExternalServices.PaymentsGateway
{
    public interface IClientApiPaymentsGateway
    {
        Task<AuthenticationResponseDto> AuthenticationAsync();

        Task<ClientDto> CreateClientAsync(ClientDto client);

        Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoice);
    }
}
