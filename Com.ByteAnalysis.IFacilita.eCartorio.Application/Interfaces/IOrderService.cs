using Com.ByteAnalysis.IFacilita.eCartorio.Application.Models;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Interfaces
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<OrderInput> CreateOrderAsync(OrderInput orderInput);

        Task<OrderInput> GetOrderAsync(string id);

        Task<OrderInput> GetOrderByCerpAsync(string cerp);

        Task<OrderInput> CreateByApplicantAsync(RequerenteInput requerenteInput);
    }
}
