using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Repository
{
    public interface IOrderRepository
    {
        List<OrderEntity> Get();

        OrderEntity Get(string id);

        OrderEntity Get(string id, CertiticateType type);

        OrderEntity Create(OrderEntity entity);

        void Update(string id, OrderEntity entity);

        void Remove(OrderEntity entity);

        void Remove(string id);
    }
}
