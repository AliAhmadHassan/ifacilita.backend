using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service
{
    public interface IOrderService
    {
        OrderEntity Create(OrderDto order);

        OrderEntity Create(OrderCertiticateDto order);

        OrderEntity Get(string id);

        OrderEntity Get(string id, CertiticateType type);

        bool ProcessCallback(string id, CertiticateType type);

        List<OrderEntity> Get();

        OrderEntity Create(OrderEntity entry);

        void Update(string id, OrderEntity entry);

        void Remove(OrderEntity entry);

        void Remove(string id);
    }
}
