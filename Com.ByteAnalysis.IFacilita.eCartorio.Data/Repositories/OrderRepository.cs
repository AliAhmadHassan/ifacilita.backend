using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Entities;
using Com.ByteAnalysis.IFacilita.eCartorio.Domain.Repositories;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IMongoDatabaseSettings settings) : base(settings)
        {
            base.SetNameCollection("order");
        }

        public async  Task<Order> GetOrderByCerpAsync(string cerp)
        {
            var result = await base.Collection.Find<Order>(t => t.Atos.Any(x=>x.Cerp == cerp)).FirstOrDefaultAsync();
            return result;
        }
    }
}
