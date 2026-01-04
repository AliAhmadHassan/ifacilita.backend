using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<Order> GetOrderByCerpAsync(string cerp);
    }
}
